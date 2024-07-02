using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;

namespace Talabat.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int>CompleteAsync();
        IGenricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntite;
    }
}
