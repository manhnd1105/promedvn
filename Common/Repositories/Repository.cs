using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Repositories
{
    public abstract class Repository<TContext, T, TKey> : IRepository
        where TContext : DbContext
        where T : class, new()
    {
        protected TContext Context { get; private set; }

        protected abstract DbSet<T> DbSet { get; }

        public Repository(TContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Context = context;
        }

        #region Creation

        public virtual T Add(T entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
            return entity;
        }

        #endregion

        #region Retrieval

        public virtual T Get(TKey id)
        {
            return DbSet.Find(id);
        }

        public virtual List<T> GetAll()
        {
            return DbSet.ToList();
        }

        #endregion

        #region Update

        public virtual void Update(T entity)
        {
            Context.SaveChanges();
        }

        #endregion

        #region Deletion
        public virtual void Delete(TKey id)
        {
            try
            {
                var entity = DbSet.Find(id);
                if (entity == null)
                {
                    throw new Exception(string.Format("There is no entity with id {0}", id));
                }
                DbSet.Remove(entity);
                //Context.SaveChanges(); //TODO temporary disable for keeping sample data on testing
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateException) { throw; }
        }

        //public void Delete(Expression<Func<T, bool>> expression)
        //{
        //    var targets = FindAll(expression);
        //    if (targets.Count > 0)
        //    {
        //        DbSet.RemoveRange(targets);
        //        Context.SaveChanges();
        //    }
        //}

        #endregion

        #region Search
        public virtual T Find(List<Expression<Func<T, bool>>> expressions)
        {
            return FindAll(expressions).FirstOrDefault();
        }

        public virtual List<T> FindAll(List<Expression<Func<T, bool>>> expressions = null, int? pageIndex = null, int? pageSize = null,
            Expression<Func<T, TKey>> orderBy = null,
            Expression<Func<T, object>>[] includes = null,
            SortingRule<T>[] sortings = null )
        {
            try
            {
                var query = DbSet.AsQueryable();
                if (expressions != null)
                {
                    foreach (var item in expressions)
                    {
                        query = query.Where(item);
                    }
                }
                if (pageIndex.HasValue && pageSize.HasValue && orderBy != null)
                {
                    query = query.OrderBy(orderBy)
                        .Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value);
                }
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query.Include(include);
                    }
                }
                if (sortings != null)
                {
                    foreach (var sorting in sortings)
                    {
                        switch (sorting.SortDirection)
                        {
                            case SortType.Descending:
                                query = query.OrderByDescending(sorting.FieldSelector);
                                break;
                            default:
                                query = query.OrderBy(sorting.FieldSelector);
                                break;
                        }
                    }
                }
                return query.ToList();
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            
        }
        #endregion

        #region Aggregation

        public int Count()
        {
            return DbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return DbSet.Count(expression);
        }

        #endregion
    }
}
