using AutoMapper;
using Core.DTOs.ProductDTOs;
using E_commerce_Api.Entities;
using E_commerce_Api.Helpers;

namespace E_commerce_Api.Profiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.TypeName, o => o.MapFrom(s => s.ProductType.Name)).
                 ForMember(d=> d.PictureUrl,o=> o.MapFrom<ProductUrlResolver>());

                //.ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                //.ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));
        }
    }
}
