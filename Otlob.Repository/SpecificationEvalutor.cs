using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Otlob.Core.Entites;
using Otlob.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        // Fun to Build Query

        //return await dbcontext.Products.Where(p=>p.Id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)


        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery ,ISpecifications<T> Spec)
        {
            var Query = inputQuery;//dbcontext.Set<T>()

            if (Spec.Criteria is not null) // P=>P.Id==id
            {
                Query = Query.Where(Spec.Criteria);// dbcontext.Set<T>().Where(P=>P.Id==id)
            }
            //P => P.ProductBrand , P => P.ProductType



            if (Spec.OrderBy is not null) 
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }

            if (Spec.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDescending);
            }


            if (Spec.IsPaginationEnabled) 
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take); 

            }







            Query = Spec.Includes.Aggregate(Query,(CurrentQuery, IncludeExpression)=>CurrentQuery.Include(IncludeExpression));

            // dbcontext.Set<T>().Where(P=>P.Id==id).Include(P => P.ProductBrand)
            // dbcontext.Set<T>().Where(P=>P.Id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)
            // 


            return Query;
        }  

    }

}

