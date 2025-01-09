using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpec)
            : base( x =>
                    (string.IsNullOrEmpty(productSpec.Name) || x.Name.ToLower().Contains(productSpec.Name)) &&
                    (!productSpec.BrandId.HasValue || x.ProductBrandId == productSpec.BrandId) &&
                    (!productSpec.TypeId.HasValue || x.ProductTypeId == productSpec.TypeId)
            )
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(productSpec.PageSize * (productSpec.PageIndex -1),productSpec.PageSize);

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;

                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddIncludes(x => x.ProductType);
            AddIncludes(x => x.ProductBrand);
        }
    }
}
