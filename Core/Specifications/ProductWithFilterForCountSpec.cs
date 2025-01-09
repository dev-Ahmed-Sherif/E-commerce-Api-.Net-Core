using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithFilterForCountSpec:BaseSpecification<Product>
    {
        public ProductWithFilterForCountSpec(ProductSpecParams productSpec) 
            : base ( x =>
                     (!productSpec.BrandId.HasValue || x.ProductBrandId == productSpec.BrandId) &&
                    (!productSpec.TypeId.HasValue || x.ProductTypeId == productSpec.TypeId)
                  )
        {
            
        }
    }
}
