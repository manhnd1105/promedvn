using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Providers
{
    public interface IProviderFactoryBase
    {
        int CacheDuration { get; }
        bool ShouldCache { get; }
    }
}
