using System.ServiceModel;
namespace Common.Contract
{
    [ServiceContract]
    public interface ICrudService<
        TSearchRequest, TSearchResponse,
        TRetrievalRequest, TRetrievalResponse,
        TCreationRequest, TCreationResponse,
        TUpdationRequest, TUpdationReponse,
        TDeletionRequest, TDeletionResponse,
        TContractRecord, TFilterRecord
        >
        where TSearchRequest : SearchRequestBase<TFilterRecord>
        where TSearchResponse : SearchResponseBase<TContractRecord>
        where TRetrievalRequest : RetrievalRequestBase
        where TRetrievalResponse : RetrievalResponseBase<TContractRecord>
        where TCreationRequest : CreationRequestBase<TContractRecord>
        where TCreationResponse : CreationResponseBase
        where TUpdationRequest : UpdationRequestBase<TContractRecord>
        where TUpdationReponse : UpdationResponseBase
        where TDeletionRequest : DeletionRequestBase
        where TDeletionResponse : DeletionResponseBase
        where TContractRecord : class
        where TFilterRecord : class
    {
        [OperationContract(Name="Search")]
        TSearchResponse Search(TSearchRequest request);

        [OperationContract(Name="Retrieve")]
        TRetrievalResponse Retrieve(TRetrievalRequest request);

        [OperationContract(Name="Create")]
        TCreationResponse Create(TCreationRequest request);

        [OperationContract(Name="Update")]
        TUpdationReponse Update(TUpdationRequest request);

        [OperationContract(Name="Delete")]
        TDeletionResponse Delete(TDeletionRequest request);
    }
}
