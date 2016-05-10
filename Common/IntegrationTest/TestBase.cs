using Common.Mock;
using Common.Providers;
using Common.Repositories;
using Common.SystemConfiguration;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace Common.IntegrationTest
{
    public abstract class TestBase<TContext>
        where TContext : DbContext
    {
        protected IProviderFactory _providerFactory { get; set; }

        public TestBase()
        {   
            InitProviderFactory();
        }

        protected abstract void InitProviderFactory();
    }
}
