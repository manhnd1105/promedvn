
using Common.Contract;
using System.Collections.Generic;
namespace Common.Services
{
    public interface ICrudBusiness<TRecord>
    {
        string Create(TRecord record);
        void Delete(string identifier);
        TRecord Retrieve(string identifier);
        List<TRecord> Search(PagingRequestRecord paging = null);
        void Update(TRecord record);
    }
}
