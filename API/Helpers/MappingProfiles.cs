using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Entities;

namespace E_commerce_Api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
           CreateMap<Product, ProductDTO>()
                .ForMember(d => d.ProductBrand,o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType,o => o.MapFrom(o => o.ProductType.Name));
        }
    }
}
