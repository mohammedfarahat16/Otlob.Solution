using Otlob.Core;
using Otlob.Core.Entites;
using Otlob.Core.Repositories;
using Otlob.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable repositories ;
        private readonly StoreContext dbContext;

        public UnitOfWork(StoreContext _dbContext)
        {
            dbContext = _dbContext;
            repositories= new Hashtable();
        }
        public async Task<int> CompeleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(dbContext);

                repositories.Add(type, Repository);

            }

            return (IGenericRepository<TEntity>) repositories[type];


        }
    }
}
