using Common.Providers;
using Common.ViewModels;

namespace Common.Business
{
    public abstract class BusinessBase
    {
        protected IProviderFactory _providerFactory;
        protected ModelConverterBase _modelConverter;
    }
}