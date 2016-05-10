
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Common.Repositories;
namespace Common.Providers
{
    public abstract class ProviderFactoryBase : IProviderFactory
    {
        protected Dictionary<Type, object> _providers = new Dictionary<Type, object>();
        public IRepositoryFactory Data { get; set; }

        public int CacheDuration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ShouldCache
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ProviderFactoryBase()
        {
            InitProviders();
        }

        public ProviderFactoryBase(IRepositoryFactory repositoryFactory)
            : this()
        {
            Data = repositoryFactory;
        }

        protected virtual void InitProviders()
        {
            try
            {
                //load list repositories by names from file
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes().Where(a => a.FullName.StartsWith("ServiceGateway.Providers") && !a.FullName.Contains("DisplayClass"));
                foreach (var item in types)
                {
                    var provider = Activator.CreateInstance(item, this);
                    _providers.Add(item, provider);
                }
            }
            catch (ReflectionTypeLoadException) { throw; }
            catch (ArgumentNullException) { throw; }
            catch (NotSupportedException) { throw; }
            catch (TargetInvocationException) { throw; }
            catch (MemberAccessException) { throw; }
            catch (TypeLoadException) { throw; }
        }

        public void Add(Type type, object provider)
        {
            _providers.Add(type, provider);
        }
        public T Get<T>()
        {
            var provider = _providers[typeof(T)];
            if (provider == null)
            {
                throw new Exception(string.Format("Provider type is not supported {0}", typeof(T).ToString()));
            }
            return (T)provider;
        }
    }
}
