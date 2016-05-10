using Common.Repositories;
using System;
namespace Common.Providers
{
    public interface IProviderFactory : IProviderFactoryBase
    {
        //IRepositoryFactory Data { get; set; }
        T Get<T>();
        void Add(Type type, object provider);
    }
}
