
namespace Common.Contract
{
    public class DeletionRequestBase : IRequest
    {
        public RequestHeaderRecord Header { get; set; }
        public string Identifier { get; set; }
    }
}
