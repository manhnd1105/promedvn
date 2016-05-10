
namespace Common.Contract
{
    public class RetrievalRequestBase : IRequest
    {
        public RequestHeaderRecord Header { get; set; }
        public string Identifier { get; set; }
    }
}
