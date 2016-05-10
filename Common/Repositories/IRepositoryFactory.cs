
using System;
namespace Common.Repositories
{
    public interface IRepositoryFactory
    {
        T Get<T>();
        void Add(Type type, object repository);
    }
}
