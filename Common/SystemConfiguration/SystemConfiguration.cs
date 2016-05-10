using System.Configuration;

namespace Common.SystemConfiguration
{
    public class SystemConfiguration
    {
        public enum TestDbStrategy
        {
            Mock,
            Test,
            Effort,
            SqlCe
        }
        public static TestDbStrategy DbStrategy
        {
            get
            {
                string environment = ConfigurationManager.AppSettings["TestDbEnvironment"];
                switch (environment)
                {
                    case "MockDb":
                        return TestDbStrategy.Mock;
                    case "TestDb":
                        return TestDbStrategy.Test;
                    case "EffortDb":
                        return TestDbStrategy.Effort;
                    case "SqlCeDb":
                        return TestDbStrategy.SqlCe;
                    default:
                        return TestDbStrategy.Effort;
                }
            }
        }
    }
}
