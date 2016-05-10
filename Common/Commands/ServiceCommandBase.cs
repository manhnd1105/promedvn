using Common.Contract;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public abstract class ServiceCommandBase<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse, new()
    {
        protected readonly Logger _log;

        protected virtual string CommandName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        protected string Caller { get; private set; }

        protected ServiceCommandBase()
        {
            _log = new LogFactory().GetCurrentClassLogger();
            Response = new TResponse();
            Response.Status = new ResponseStatusRecord();
            Response.Status.IsSuccessful = false;
        }

        public TRequest Request { get; private set; }

        protected TResponse Response { get; private set; }

        protected abstract void Validate(TRequest request);

        protected abstract bool OnExecute(TResponse response);

        public virtual TResponse Execute(TRequest request)
        {
            Request = request;

            HandleBeforeExecute();
            try
            {
                this.Validate(request);

                bool executeResult = this.OnExecute(Response);

                Response.Status.IsSuccessful = executeResult;
            }
            catch (Exception ex)
            {
                this.HandleException(true, ex);
            }
            finally
            {
                _log.Debug(String.Format("Ending {0}", CommandName));
            }
            HandleAfterExecute();
            return Response;
        }

        protected virtual void HandleBeforeExecute()
        {
            if (Request != null)
            {
                if (Request.Header != null)
                {
                    Caller = Request.Header.CallerName;
                }
                else
                {
                    Caller = "No Header";
                }
            }
            else
            {
                Caller = "No Request";
            }
            _log.Debug(String.Format("Beginning {0} - Caller: {1}", CommandName, Caller));
        }

        protected virtual void HandleAfterExecute()
        {
            if (Response.Status.IsSuccessful)
            {
                _log.Debug(String.Format("Ending {0} - Success - Caller: {1}", CommandName, Caller));
            }
            else
            {
                _log.Debug(String.Format("Ending {0} - Failure - Caller: {1}; Error: {2};", CommandName, Caller, Response.Status.ErrorMessage));
            }
        }

        protected virtual void HandleException(bool errorLogging, Exception e)
        {
            if (errorLogging)
            {
                _log.Error(e, "Error executing " + CommandName);
            }
            else
            {
                //_log.Error("Error executing " + CommandName, e);
            }
            SetResponseStatus(false, e.Message, -1);
        }

        protected virtual void SetResponseStatus(bool isSuccessful, string errorDescription, int errorCode)
        {
            Response.Status.IsSuccessful = isSuccessful;
            Response.Status.ErrorMessage = errorDescription;
        }
    }
}
