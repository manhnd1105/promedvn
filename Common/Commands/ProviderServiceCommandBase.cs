using Common.Contract;
using Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public abstract class ProviderServiceCommandBase<TFactory, TRequest, TResponse> : ServiceCommandBase<TRequest, TResponse>
        where TFactory : IProviderFactoryBase
        where TRequest : IRequest
        where TResponse : IResponse, new()
    {
        protected TFactory _providerFactory;

        public ProviderServiceCommandBase(TFactory providerFactory)
            : base()
        {
            if (providerFactory == null) throw new ArgumentNullException("providerFactory");

            _providerFactory = providerFactory;
        }
    }
}
