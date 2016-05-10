using Effort;
using System;
using System.Data.EntityClient;

namespace Common.IntegrationTest
{
    public class EffortDatabaseStrategy<TContext> : ITestDatabaseStrategy<TContext>
        where TContext : class
    {
        private EntityConnection _connection;
        public TContext CreateContext()
        {
            string connectionString = "name=PccContext";
            if (_connection == null)
            {
                _connection = EntityConnectionFactory.CreateTransient(connectionString);
            }
            var context = EntityConnectionFactory.CreateTransient(connectionString);
            return (TContext)Activator.CreateInstance(typeof(TContext), context);
        }
    }
}
