using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ServiceFactory
    {
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public ServiceFactory()
        {
            try
            {
                //load list repositories by names from file
                var assembly = Assembly.GetExecutingAssembly();
                var types = assembly.GetTypes().Where(a => a.FullName.StartsWith("ServiceFacade.Wcf"));
                foreach (var item in types)
                {
                    var service = Activator.CreateInstance(item);
                    _services.Add(item, service);
                }
            }
            catch (ReflectionTypeLoadException) { throw; }
            catch (ArgumentNullException) { throw; }
            catch (NotSupportedException) { throw; }
            catch (TargetInvocationException) { throw; }
            catch (MemberAccessException) { throw; }
            catch (TypeLoadException) { throw; }
        }
        public void Add(Type type, object repository)
        {
            _services.Add(type, repository);
        }
        public T Get<T>()
        {
            var service = _services[typeof(T)];
            if (service == null)
            {
                throw new Exception(string.Format("Service type is not supported {0}", typeof(T).ToString()));
            }
            return (T)service;
        }
    }
}
