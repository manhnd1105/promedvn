using System.Collections.Generic;

namespace Common.Contract
{
    public class SearchResponseBase<T> : IResponse
    {
        public ResponseStatusRecord Status { get; set; }
        public PagingResponseRecord Paging { get; set; }
        public List<T> Records { get; set; }
    }
}
