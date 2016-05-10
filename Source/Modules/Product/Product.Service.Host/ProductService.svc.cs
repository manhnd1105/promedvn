using Common.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Common.Contract.Requests;
using Common.Contract.Responses;

namespace Product.Service.Host
{
    public class ProductService : IProductService
    {
        public CreateProductResponse Create(CreateProductRequest request)
        {
            throw new NotImplementedException();
        }

        public DeleteProductResponse Delete(DeleteProductRequest request)
        {
            throw new NotImplementedException();
        }

        public RetrieveProductResponse Retrieve(RetrieveProductRequest request)
        {
            throw new NotImplementedException();
        }

        public SearchProductsResponse Search(SearchProductsRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateProductResponse Update(UpdateProductRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
