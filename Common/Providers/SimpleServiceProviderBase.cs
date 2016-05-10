using Common.Contract;
using System;
using System.ServiceModel;

namespace Common.Providers
{
    public abstract class SimpleServiceProviderBase<TService, TFactory> : ProviderBase<TService, TFactory>
        where TService : class
        where TFactory : IProviderFactory
    {
        private SimpleWcfClient<TService> _client;
        private string _endpointConfigurationName;

        public SimpleServiceProviderBase(TFactory providerFactory, string endpointConfigurationName) : base(providerFactory)
        {
            _endpointConfigurationName = endpointConfigurationName;
        }

        public SimpleServiceProviderBase(TFactory providerFactory) : this(providerFactory, "") { }

        protected override TService CreateInstance()
        {
            TService instance = null;
            try
            {
                _client = new SimpleWcfClient<TService>(_endpointConfigurationName);
                instance = _client.Proxy;
                if (instance == null)
                {
                    throw new Exception(String.Format("Failed to create instance of type {0} with endpoint {1}", typeof(TService), _endpointConfigurationName));
                }
                return instance;
            }
            catch (Exception) {throw;}
        }

        protected TResponse CallService<TResponse>(Func<TResponse> method)
            where TResponse : class, IResponse
        {
            try
            {
                var result = method();
                var serviceName = typeof(TService).Name;
                if (result != null && result.Status != null && result.Status.IsSuccessful)
                {
                    _log.Info(String.Format("Service call {0} successful", serviceName));
                }
                else
                {
                    string errorDetail;
                    if (result == null)
                    {
                        errorDetail = "Result is null";
                    }
                    else if (result.Status == null)
                    {
                        errorDetail = "Status is null";
                    }
                    else if (String.IsNullOrWhiteSpace(result.Status.ErrorMessage))
                    {
                        errorDetail = "ErrorMessage is null";
                    }
                    else
                    {
                        errorDetail = result.Status.ErrorMessage;
                    }
                    _log.Error(String.Format("Service call {0} was unsuccessful - {1}", serviceName, errorDetail));
                }
                return result;
            }
            catch (FaultException fault)
            {
                //HandleFault(fault);
            }
            catch (CommunicationException communication)
            {
                Monitor(communication.ToString());
            }
            catch (TimeoutException timeout)
            {
                Monitor(timeout.ToString());
            }
            return null;
        }

        protected virtual void VerifyResponse(IResponse response)
        {
            if (response == null)
            {
                throw new Exception("No Response");
            }
            if (!response.Status.IsSuccessful)
            {
                throw new Exception("Response failed");
            }
        }
    }
}
