using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithFiltersForSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForSpecification(ProductSpecParams productParams) 
            : base ( x =>
                  (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                  (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) 
                  )
        {
            
        }
    }
}
