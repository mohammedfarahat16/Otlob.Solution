using Otlob.Core.Entites;
using Otlob.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        #region Without Specifications

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);


        #endregion

        #region With Specifications

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);

        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);


        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        #endregion



        Task Add(T item);

        void Delete(T item);
        void Update(T item);


    }
}
