using Core.Entities;
using Core.Interfaces;
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
    public class ProductController : ControllerBase
    {
        // Old 15 10 2024
        private readonly IProductRepository _productRepository;

        // Implement Generic Repo 15 10 2024
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

        public ProductController(
            IProductRepository productRepository, 
            IGenericRepository<Product> productRepo, 
            IGenericRepository<ProductBrand> productBrandRepo,
            IGenericRepository<ProductType> productTypeRepo
            )
        {
            _productRepository = productRepository;
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts() 
        {
            // Without Generic Repo
            //IReadOnlyList<Product> products = await _productRepository.GetProductsAsync();

            // Using Generic Repo
            IReadOnlyList<Product> products = await _productRepo.ListAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            //Product? product = await _productRepository.GetProductByIdAsync(id);
            Product? product = await _productRepo.GetByIdAsync(id);
            return Ok(product);
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
