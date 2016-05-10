
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
namespace Common.Repositories
{
    public abstract class RepositoryFactoryBase : IRepositoryFactory
    {
        protected Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public RepositoryFactoryBase(DbContext context)
        {
            InitRepositories(context);
        }
        protected abstract void InitRepositories(DbContext context);

        public void Add(Type type, object repository)
        {
            _repositories.Add(type, repository);
        }
        public T Get<T>()
        {
            var repository = _repositories[typeof(T)];
            if (repository == null)
            {
                throw new Exception(string.Format("Repository type is not supported {0}", typeof(T).ToString()));
            }
            return (T)repository;
        }
    }
}
