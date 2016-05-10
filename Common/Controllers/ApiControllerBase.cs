using Common.Providers;
using Common.ViewModels;
using NLog;
using System.Web.Http;

namespace Common.Controllers
{
    public abstract class ApiControllerBase<TProvider> : ApiController
        where TProvider : IProvider
    {
        protected TProvider _provider;
        protected ModelConverterBase _modelFactory = null;
        protected IProviderFactory _providerFactory = null;
        protected Logger _log;

        public ApiControllerBase()
        {
            _log = new LogFactory().GetCurrentClassLogger();
        }

        public ApiControllerBase(IProviderFactory providerFactory) : this()
        {
            _providerFactory = providerFactory;
            _provider = _providerFactory.Get<TProvider>();
        }
    }
}