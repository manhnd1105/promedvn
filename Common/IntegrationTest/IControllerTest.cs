
using System.Data.Entity;
namespace Common.IntegrationTest
{
    public interface IControllerTest
    {
        void Post_Success();
        void Put_Success();
        void Get_Success();
        void Gets_Success();
        void Delete_Success();
    }
}
