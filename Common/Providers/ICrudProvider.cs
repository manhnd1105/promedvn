using Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Providers
{
    public interface ICrudProvider<TRecord> : IProvider
        where TRecord : class
    {
        void Create(TRecord record);
        void Update(TRecord record);
        TRecord Retrieve(string identifier);
        List<TRecord> Search(PagingRequestRecord pagination = null);
        void Delete(string identifier);
    }
}
