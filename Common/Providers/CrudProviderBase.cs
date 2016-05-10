using Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Providers
{
    public abstract class CrudProviderBase<TService, TFactory, TRecord, TFilterRecord> : 
        SimpleServiceProviderBase<TService, TFactory>, ICrudProvider<TRecord>
        where TFilterRecord : class
        where TFactory : IProviderFactory
        where TRecord : class
        where TService : class, ICrudService<
            SearchRequestBase<TFilterRecord>, SearchResponseBase<TRecord>,
            RetrievalRequestBase, RetrievalResponseBase<TRecord>,
            CreationRequestBase<TRecord>, CreationResponseBase,
            UpdationRequestBase<TRecord>, UpdationResponseBase,
            DeletionRequestBase, DeletionResponseBase,
            TRecord, TFilterRecord>
    {
        public CrudProviderBase(TFactory providerFactory, string endpoint) : base(providerFactory, endpoint) { }
        public CrudProviderBase(TFactory providerFactory) : base(providerFactory) { }

        public void Create(TRecord record)
        {
            var request = new CreationRequestBase<TRecord>()
            {
                Header = new RequestHeaderRecord()
                {
                    CallerName = this.GetType().ToString()
                },
                Record = record
            };
            var response = CallService<CreationResponseBase>(() => CreateInstance().Create(request));
            VerifyResponse(response);
        }

        public void Update(TRecord record)
        {
            var request = new UpdationRequestBase<TRecord>()
            {
                Header = new RequestHeaderRecord()
                {
                    CallerName = this.GetType().ToString()
                },
                Record = record
            };
            var response = CallService<UpdationResponseBase>(() => CreateInstance().Update(request));
            VerifyResponse(response);
        }

        public TRecord Retrieve(string identifier)
        {
            var request = new RetrievalRequestBase()
            {
                Header = new RequestHeaderRecord()
                {
                    CallerName = this.GetType().ToString()
                },
                Identifier = identifier
            };
            var response = CallService<RetrievalResponseBase<TRecord>>(() => CreateInstance().Retrieve(request));
            VerifyResponse(response);
            return response.Record;
        }

        public List<TRecord> Search(PagingRequestRecord pagination = null)
        {
            var request = new SearchRequestBase<TFilterRecord>()
            {
                Header = new RequestHeaderRecord()
                {
                    CallerName = this.GetType().ToString()
                },
                Paging = pagination
            };
            var response = CallService<SearchResponseBase<TRecord>>(() => CreateInstance().Search(request));
            VerifyResponse(response);
            return response.Records;
        }

        public void Delete(string identifier)
        {
            var request = new DeletionRequestBase()
            {
                Header = new RequestHeaderRecord()
                {
                    CallerName = this.GetType().ToString()
                },
                Identifier = identifier
            };
            var response = CallService<DeletionResponseBase>(() => CreateInstance().Delete(request));
            VerifyResponse(response);
        }
    }
}
