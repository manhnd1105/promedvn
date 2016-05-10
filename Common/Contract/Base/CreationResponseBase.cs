
namespace Common.Contract
{
    public class CreationResponseBase : IResponse
    {
        public ResponseStatusRecord Status { get; set; }
        public string Identifier { get; set; }
    }
}
