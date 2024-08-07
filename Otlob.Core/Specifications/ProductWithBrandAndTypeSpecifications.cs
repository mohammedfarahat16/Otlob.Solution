using Otlob.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications :BaseSpecifications<Product>
    {

        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params)
                :base(p=>
                (string.IsNullOrEmpty( Params.Search)||p.Name.ToLower().Contains(Params.Search.ToLower()))
                &&
                (!Params.TypeId.HasValue||p.ProductTypeId==Params.TypeId)
                &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId))
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort) 
                {
                    case "PriceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }


            ApplyPagination(Params.PageSize*(Params.PageIndex-1),Params.PageSize);


        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

        }
    }
}
