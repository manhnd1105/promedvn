
namespace Common.Contract
{
    public class UpdationRequestBase<T> : IRequest
    {
        public RequestHeaderRecord Header { get; set; }
        public T Record { get; set; }
    }
}
