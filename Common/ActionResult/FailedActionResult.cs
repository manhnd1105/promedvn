using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Net;

namespace Common.ActionResult
{
    public class FailedActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _errors;

        public FailedActionResult(HttpRequestMessage request, string errors)
        {
            _request = request;
            _errors = errors;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.ExpectationFailed);
            response.ReasonPhrase = _errors.Replace(Environment.NewLine, String.Empty);
            return Task.FromResult(response);
        }
    }
}
