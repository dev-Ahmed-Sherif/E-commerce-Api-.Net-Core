using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Entities;

namespace E_commerce_Api.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config) 
        { 
            _config = config;
        }
        public string Resolve(Product source,ProductDTO destination,string dataMember, ResolutionContext context) 
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }
            return string.Empty;
        }
    }
}
