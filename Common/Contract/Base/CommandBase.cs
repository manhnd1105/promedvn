using Common.Contract.Exception;
using NLog;

namespace Common.Contract
{
	public abstract class CommandBase<TRequest, TResponse>
		where TRequest: IRequest
		where TResponse: IResponse, new()
	{
		protected readonly Logger Log = LogManager.GetCurrentClassLogger();
		protected virtual string CommandName
		{
			get
			{
				return this.GetType().Name;
			}
		}

		protected string Caller { get; private set; }

		public CommandBase()
		{
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
			catch (ProcessingException ex)
			{
				this.HandleException(false, ex);
			}
			catch (ValidationException ex)
			{
				this.HandleException(false, ex);
			}
			catch (ConfigurationException ex)
			{
				this.HandleException(true, ex);
			}
			catch (System.Exception ex)
			{
				this.HandleException(true ,ex);
			}
			finally
			{
				Log.Debug("Ending {0}", CommandName);
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
			Log.Debug("Beginning {0} - Caller: {1}", CommandName, Caller);
		}

		protected virtual void HandleAfterExecute()
		{
			if (Response.Status.IsSuccessful)
			{
				Log.Debug("Ending {0} - Success - Caller: {1}", CommandName, Caller);
			}
			else
			{
				Log.Debug("Ending {0} - Failure - Caller: {1}; Error: {2};", CommandName, Caller, Response.Status.ErrorMessage);
			}
		}

		protected virtual void HandleException(bool errorLogging, System.Exception e)
		{
			if (errorLogging)
			{
				Log.Error(e, "Error while executing " + CommandName);
			}
			else
			{
				Log.Trace(e, "Error while executing " + CommandName);
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
