using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context) 
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            // Lazy loading Data
            //return await _context.Products.ToListAsync();

            // Eager loading Data 
            // Use AsSplitQuery() To Prevent Cross Join in Select Data 
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            // Lazy Loading Data
            ////return await _context.Products.FindAsync(id);

            // Eager loading Data
            // Use AsSplitQuery() To Prevent Cross Join in Select Data 
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();

        }
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }
    }
}
