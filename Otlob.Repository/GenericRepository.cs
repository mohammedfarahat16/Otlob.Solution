using Microsoft.EntityFrameworkCore;
using Otlob.Core.Entites;
using Otlob.Core.Repositories;
using Otlob.Core.Specifications;
using Otlob.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbcontext;

        public GenericRepository(StoreContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }
        #region Without Specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IReadOnlyList<T>)await dbcontext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType)
                                      .ToListAsync();
            else
                return await dbcontext.Set<T>().ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbcontext.Set<T>().FindAsync(id);
            //return await dbcontext.Products.Where(p=>p.Id==id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        }

        #endregion


        #region With Specifications


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();

        }
        #endregion

        private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvalutor<T>.GetQuery(dbcontext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }

        public async Task Add(T item)
        {
            await dbcontext.Set<T>().AddAsync(item);
        }

        public void Delete(T item)
        {
            dbcontext.Set<T>().Remove(item); 
        }

        public void Update(T item)
        {
            dbcontext.Set<T>().Update(item);
        }
    }
}
