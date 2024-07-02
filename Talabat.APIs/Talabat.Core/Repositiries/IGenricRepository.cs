using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Secifications;

namespace Talabat.Core.Repositiries
{
    public interface IGenricRepository<T> where T : BaseEntite
    {
        #region without specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetbyIdAsync(int id);
        Task AddAsync(T entity);
        #endregion

        #region with specifications

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T> GetEntityWithSpecAsync( ISpecifications<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);
        #endregion
        
        void Update(T entity);
        void Delete(T entity);
    }
}
