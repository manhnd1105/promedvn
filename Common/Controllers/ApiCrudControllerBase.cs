using Common.Contract;
using Common.Providers;
using Common.ViewModels;
using Common.ActionResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Common.Controllers
{
    public abstract class ApiCrudControllerBase<TProvider, TRecord> : ApiControllerBase<TProvider>
        where TProvider : ICrudProvider<TRecord>
        where TRecord : class
    {
        public ApiCrudControllerBase() : base()
        { }
        public ApiCrudControllerBase(IProviderFactory providerFactory) : base(providerFactory)
        { }

        public IHttpActionResult Search<TViewModel>(int? page, int? pageSize)
            where TViewModel : class
        {
            try
            {
                PagingRequestRecord pagination = null;
                if (page.HasValue && pageSize.HasValue)
                {
                    pagination = new PagingRequestRecord()
                    {
                        PageIndex = page.Value,
                        PageSize = pageSize.Value
                    };
                }
                var records = _provider.Search(pagination);
                return Ok(new Pagination<TViewModel>()
                    {
                        Page = page.Value,
                        TotalCount = records.Count,
                        TotalPages = (int)Math.Ceiling((decimal)records.Count / pageSize.Value),
                        Items = ConvertToViewModels<TViewModel>(records)
                    });
            }
            catch (Exception e)
            {
                _log.Error(e, "ApiCrudControllerBase.Search");
                return NotFound();
            }
        }

        public IHttpActionResult Retrieve<TViewModel>(string identifier)
            where TViewModel : class
        {
            try
            {
                var record = _provider.Retrieve(identifier);
                return Ok(ConvertToViewModel<TViewModel>(record));
            }
            catch (Exception e)
            {
                _log.Error(e, "ApiCrudControllerBase.Retrieve");
                return new FailedActionResult(
                    request: Request,
                    errors: e.ToString()
                    );
            }
        }

        public IHttpActionResult Create<TViewModel>(TViewModel model)
            where TViewModel : class
        {
            try
            {
                TRecord record = ConvertToRecord<TViewModel>(model);
                _provider.Create(record);
                return Ok();
            }
            catch (Exception e)
            {
                _log.Error(e, "ApiCrudControllerBase.Create");
                return new FailedActionResult(
                    request: Request,
                    errors: e.ToString()
                    );
            }
        }

        public IHttpActionResult Update<TViewModel>(TViewModel model)
            where TViewModel : class
        {
            try
            {
                TRecord record = ConvertToRecord<TViewModel>(model);
                _provider.Update(record);
                return Ok();
            }
            catch (Exception e)
            {
                _log.Error(e, "ApiCrudControllerBase.Update");
                return new FailedActionResult(
                    request: Request,
                    errors: e.ToString()
                    );
            }
        }

        public IHttpActionResult Remove(string identifier)
        {
            try
            {
                _provider.Delete(identifier);
                return Ok();
            }
            catch (Exception e)
            {
                _log.Error(e, "ApiCrudControllerBase.Remove");
                return new FailedActionResult(
                    request: Request,
                    errors: e.ToString()
                    );
            }
        }

        protected virtual TViewModel ConvertToViewModel<TViewModel>(TRecord record)
            where TViewModel : class
        {
            if (_modelFactory == null) throw new ArgumentNullException("ModelConverter");
            try
            {
                return _modelFactory.Convert<TRecord, TViewModel>(record);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual List<TViewModel> ConvertToViewModels<TViewModel>(List<TRecord> records)
            where TViewModel : class
        {
            if (_modelFactory == null) throw new ArgumentNullException("ModelConverter");
            try
            {
                return _modelFactory.Converts<TRecord, TViewModel>(records);

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual TRecord ConvertToRecord<TViewModel>(TViewModel model)
            where TViewModel : class
        {
            if (_modelFactory == null) throw new ArgumentNullException("ModelConverter");
            try
            {
                return _modelFactory.Convert<TViewModel, TRecord>(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}