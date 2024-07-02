using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;
using Talabat.Core.Secifications;
using Talabat.Repositiory.Data;

namespace Talabat.Repositiory
{
    public class GenericRepository<T> : IGenricRepository<T> where T : BaseEntite
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #region without specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _dbcontext.Products.Include(p => p.productBrand).Include(p => p.productType).ToListAsync();
            }
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetbyIdAsync(int id)
        {
            // await _dbcontext.Set<T>().Where(d=>d.Id == id).FirstOrDefaultAsync();  //search in database direct
            // return _dbcontext.Set<T>().Where(p=>p.Id == id).Include(p => p.productBrand).Include(p => p.productType).ToListAsync();
            return await _dbcontext.Set<T>().FindAsync(id);
        }
        
        #endregion
        #region with specification
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpeicifations(spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpeicifations(spec).FirstOrDefaultAsync();

        }
        private IQueryable<T> ApplySpeicifations(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbcontext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpeicifations(spec).CountAsync();
        }


        #endregion
        public async Task AddAsync(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
        }
    }
}
