using Otlob.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications
{
    public interface ISpecifications <T> where T : BaseEntity
    {
        // Sign for Property for Where Condetion [Where(p=>p.id==id)]
        public Expression<Func<T,bool>> Criteria { get; set; }

        // Sign for Property for List Of Include  [Include(p=>p.ProductPrand).Include(p=>p.ProductType)]

        public List<Expression<Func<T,object>>> Includes { get; set; }

        //OrderBy
        public Expression<Func<T,object>> OrderBy { get; set; }

        // OrderBy Desc

        public Expression<Func<T, object>> OrderByDescending   { get; set; }

        //Skip

        public int Skip { get; set; }


        //Take
        public int Take { get; set; }


        public bool IsPaginationEnabled { get; set; }






    }
}
