using System;
using System.ServiceModel;

namespace Common.Providers
{
    public class SimpleWcfClient<TChannel> : ClientBase<TChannel> 
        where TChannel : class
    {
        private bool _disposed = false;

        public SimpleWcfClient()
            : base(String.Empty)
        {
        }

        public SimpleWcfClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public TChannel Proxy
        {
            get { return this.Channel; }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this.State == CommunicationState.Faulted)
                    {
                        base.Abort();
                    }
                    else
                    {
                        try
                        {
                            base.Close();
                        }
                        catch
                        {
                            base.Abort();
                        }
                    }
                    _disposed = true;
                }
            }
        }
    }
}
