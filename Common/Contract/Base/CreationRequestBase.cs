
namespace Common.Contract
{
    public class CreationRequestBase<T> : IRequest
    {
        public RequestHeaderRecord Header { get; set; }
        public T Record { get; set; }
    }
}
