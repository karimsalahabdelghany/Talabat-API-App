using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositiries;
using Talabat.Repositiory.Data;

namespace Talabat.Repositiory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;
        private Hashtable _repositories;   // reference should refer ro object

        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
          return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _dbcontext.DisposeAsync();
        }

        public IGenricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntite
        {
            var type = typeof(TEntity).Name;  //Product , Order , DeliveryMethod
            if(!_repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity>(_dbcontext);
                _repositories.Add(type,Repository);
                return Repository;
            }
            return (IGenricRepository<TEntity>) _repositories[type];
        }
    }
}
