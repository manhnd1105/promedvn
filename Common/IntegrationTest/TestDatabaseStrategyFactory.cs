
using System;
namespace Common.IntegrationTest
{
    public class TestDatabaseStrategyFactory
    {
        public static ITestDatabaseStrategy<TContext> CreateFakeDatabase<TContext>()
            where TContext : class
        {
            return new EffortDatabaseStrategy<TContext>();
        }

        public static ITestDatabaseStrategy<TContext> CreateRealDatabase<TContext>()
            where TContext : class
        {
            //return new SqlCeDatabaseStrategy();
            throw new NotImplementedException();
        }
    }
}
