
namespace Common.Contract
{
    public class RetrievalResponseBase<T> : IResponse
    {
        public ResponseStatusRecord Status { get; set; }
        public T Record { get; set; }
    }
}
