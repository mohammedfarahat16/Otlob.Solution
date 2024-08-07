using Otlob.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications
{
    public class ProductWithFiltrationForCountSpec : BaseSpecifications<Product>
    {
        public ProductWithFiltrationForCountSpec(ProductSpecParams Params) : base(p =>
        (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search.ToLower()))
                &&
                (!Params.TypeId.HasValue||p.ProductTypeId==Params.TypeId)
                &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId))
        {
            
        }
    }
}
