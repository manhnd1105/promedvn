namespace Common.IntegrationTest
{
    public interface ITestDatabaseStrategy<TContext>
        where TContext : class
    {
        TContext CreateContext();
    }
}
