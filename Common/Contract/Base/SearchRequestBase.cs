
namespace Common.Contract
{
    public class SearchRequestBase<T> : IRequest
    {
        public RequestHeaderRecord Header { get; set; }
        public PagingRequestRecord Paging { get; set; }
        public T FilterRecord { get; set; }
        public SearchRequestBase()
        {
            Paging = new PagingRequestRecord()
            {
                PageIndex = 0,
                PageSize = 500
            };
        }
    }
}
