using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Common.Mock
{
    public class DbSetMocker
    {
        public DbSet<T> CreateMockDbSet<T>(MockRepository mocks)
            where T : class
        {
            var generatedID = 0;
            var list = new List<T>();
            var query = list.AsQueryable();
            var mockSet = mocks.Create<DbSet<T>>(MockBehavior.Strict);
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(() => query.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(() => query.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(() => query.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => query.GetEnumerator());
            mockSet
                .Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns((object[] e) =>
                {
                    if (e.Length == 1)
                    {
                        var type = typeof(T);
                        var property = GetIDProperty(type);
                        if (property != null && property.CanRead)
                        {
                            foreach (T entity in list)
                            {
                                var id = property.GetValue(entity, null);
                                if (id != null && id.Equals(e[0]))
                                {
                                    return entity;
                                }
                            }
                        }
                    }
                    return null;
                });
            mockSet
                .Setup(m => m.Add(It.IsAny<T>()))
                .Callback((T e) =>
                {
                    SetID(e, ++generatedID);
                    list.Add(e);
                })
                .Returns((T e) => e);

            var returnList = new List<T>();
            //mockSet
            //    .Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
            //    .Callback((IEnumerable<T> e) =>
            //    {
            //        foreach (var entity in e)
            //        {
            //            if (list.Remove(entity))
            //            {
            //                returnList.Add(entity);
            //            }
            //        }
            //    })
            //    .Returns((T e) =>
            //    {
            //        var tempList = returnList.ToList();
            //        returnList.Clear();
            //        return tempList;
            //    });

            mockSet
                .Setup(m => m.Remove(It.IsAny<T>()))
                .Callback((T e) =>
                {
                    list.Remove(e);
                })
                .Returns((T e) =>
                {
                    return e;
                });

            return mockSet.Object;
        }
        private void SetID(object entity, int id)
        {
            var type = entity.GetType();
            var property = GetIDProperty(type);
            if (property != null && property.CanWrite)
            {
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Int32) || propertyType == typeof(Int64))
                {
                    property.SetValue(entity, id, null);
                }
                else if (propertyType == typeof(String))
                {
                    property.SetValue(entity, id.ToString(), null);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported ID property type - " + propertyType);
                }
            }
        }
        private static PropertyInfo GetIDProperty(Type type)
        {
            var property = type.GetProperty(type.Name + "ID", BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                property = type.GetProperty("ID", BindingFlags.Public | BindingFlags.Instance);
            }
            return property;
        }
    }
}
