using NLog;
using System;

namespace Common.Providers
{
    public abstract class ProviderBase<TContract, TFactory>
        where TContract : class
        where TFactory : IProviderFactory
    {
        protected Logger _log;
        protected TFactory _providerFactory;

        public ProviderBase(TFactory providerFactory)
        {
            if (providerFactory == null) throw new ArgumentNullException("providerFactory");
            _providerFactory = providerFactory;
            _log = new LogFactory().GetCurrentClassLogger();
        }

        protected abstract TContract CreateInstance();

        protected virtual void Monitor(string message)
        {
            _log.Info(String.Format("Monitoring message {0}", message));
        }
    }
}
