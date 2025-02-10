using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using E_commerce_Api.Controllers;
using E_commerce_Api.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        // Old 15 10 2024
        private readonly IProductRepository _productRepository;

        // Implement Generic Repo 15 10 2024
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductController(
            IProductRepository productRepository,
            IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper
            )
        {
            _productRepository = productRepository;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        //public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecParams productSpec)
        {
            // Without Generic Repo
            //IReadOnlyList<Product> products = await _productRepository.GetProductsAsync();

            // Using Generic Repo
            // Without Specification ( Includes )
            //IReadOnlyList<Product> products = await _productRepo.ListAllAsync();
            var spec = new ProductWithTypesAndBrandsSpecification(productSpec);
            var countSpec = new ProductWithFilterForCountSpec(productSpec);

            var totalItems = await _productRepo.CountAsync(countSpec);

            IReadOnlyList<Product> products = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            //return Ok(products);

            // 15 12 2024 This work fine without auto Mapper Package
            //return products.Select(product => new ProductDTO()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    PictureUrl = product.PictureUrl,
            //    //ProductBrand = product.ProductBrand.Name,
            //    //ProductType = product.ProductType.Name,
            //    BrandName = product.ProductBrand.Name,
            //    TypeName = product.ProductType.Name,
            //}).ToList();

            // ProjectTo need IQueryable input 
            //return products.Select(product => _mapper.ProjectTo<ProductDTO>(product)).ToList();

            return Ok( new Pagination<ProductDto>(productSpec.PageIndex,productSpec.PageSize,totalItems,data) );

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            // Without Generic Repo
            //Product? product = await _productRepository.GetProductByIdAsync(id);

            // Using Generic Repo
            // Without Specification ( Includes )
            //Product? product = await _productRepo.GetByIdAsync(id);
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            Product? product = await _productRepo.GetEntityWithSpec(spec);
            //return Ok(product);
            //return new ProductDTO()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    PictureUrl = product.PictureUrl,
            //    BrandName = product.ProductBrand.Name,
            //    TypeName = product.ProductType.Name,
            //};
            return Ok( _mapper.Map<Product, ProductDto>(product));
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            //IReadOnlyList<ProductType> productTypes = await _productRepository.GetProductTypesAsync();
            IReadOnlyList<ProductType> productTypes = await _productTypeRepo.ListAllAsync();
            return Ok(productTypes);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            //IReadOnlyList<ProductBrand> productBrands = await _productRepository.GetProductBrandsAsync();
            IReadOnlyList<ProductBrand> productBrands = await _productBrandRepo.ListAllAsync();
            return Ok(productBrands);
        }
    }
}
