
using Common.Providers;
using Common.Repositories;
using Common.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Common.Services
{
    public abstract class ServiceFacadeBase
    {
        protected Logger _log;
        protected IProviderFactory _providerFactory = null;
        protected ModelConverterBase _modelFactory = null;
        public ServiceFacadeBase(IProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
            _log = new LogFactory().GetCurrentClassLogger();
        }
        protected Expression<Func<T, bool>> AndAll<T>(
            IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            try
            {
                if (expressions == null)
                {
                    throw new ArgumentNullException("expressions");
                }
                if (expressions.Count() == 0)
                {
                    return t => true;
                }
                Type delegateType =
                    typeof(Func<,>)
                    .GetGenericTypeDefinition()
                    .MakeGenericType(
                        new[] 
                    {
                        typeof(T),
                        typeof(bool) 
                    }
                    );
                var combined = expressions
                                   .Cast<Expression>()
                                   .Aggregate((e1, e2) => Expression.AndAlso(e1, e2));
                return (Expression<Func<T, bool>>)Expression.Lambda(delegateType, combined);
            }
            catch (InvalidOperationException) { throw; }
            catch (NotSupportedException) { throw; }
            catch (ArgumentNullException) { throw; }
        }

        protected virtual TEntity ConvertToEntity<TRecord, TEntity>(TRecord record)
            where TRecord : class
            where TEntity : class
        {
            return _modelFactory.Convert<TRecord, TEntity>(record);
        }

        protected virtual List<TEntity> ConvertToEntities<TRecord, TEntity>(List<TRecord> record)
            where TRecord : class
            where TEntity : class
        {
            return _modelFactory.Converts<TRecord, TEntity>(record);
        }

        protected virtual TRecord ConvertToRecord<TEntity, TRecord>(TEntity entity)
            where TRecord : class
            where TEntity : class
        {
            return _modelFactory.Convert<TEntity, TRecord>(entity);
        }

        protected virtual List<TRecord> ConvertToRecords<TEntity, TRecord>(List<TEntity> entities)
            where TRecord : class
            where TEntity : class
        {
            return _modelFactory.Converts<TEntity, TRecord>(entities);
        }
    }
}
