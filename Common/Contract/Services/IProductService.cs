using Common.Contract.Records;
using Common.Contract.Requests;
using Common.Contract.Responses;
using System.ServiceModel;

namespace Common.Contract.Services
{
    [ServiceContract]
    public interface IProductService : ICrudService<
            SearchProductsRequest, SearchProductsResponse,
            RetrieveProductRequest, RetrieveProductResponse,
            CreateProductRequest, CreateProductResponse,
            UpdateProductRequest, UpdateProductResponse,
            DeleteProductRequest, DeleteProductResponse,
            ProductRecord, ProductFilterRecord
        >
    {
    }
}
