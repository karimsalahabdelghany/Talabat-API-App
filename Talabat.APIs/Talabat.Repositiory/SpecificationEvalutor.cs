using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Models;
using Talabat.Core.Secifications;

namespace Talabat.Repositiory
{
    public static class SpecificationEvalutor <T> where T :BaseEntite
    {
        //Functtion to build query
        //_dbcontext.Set<T>().Where(p=>p.Id == id).Include(p => p.productBrand).Include(p => p.productType).ToListAsync();
        public static IQueryable<T>GetQuery(IQueryable<T> inputquery,ISpecifications<T> spec)
        {
            var Query = inputquery;  //_dbcontext.Set<T>()

            if(spec.Criteria is not null)    //p => p.Id == id
            {
                Query =Query.Where(spec.Criteria);   //_dbcontext.Set<T>().Where(p => p.Id == id)
            }

            if(spec.OrderBy is not null)
            {
                Query = Query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDecsending is not null)
            {
                Query = Query.OrderByDescending(spec.OrderByDecsending);
            }
            if (spec.IsPageniationEnabled)
            {
                Query = Query.Skip(spec.Skip).Take(spec.Take);  
            }
            //p => p.productBrand, p => p.productType
            Query = spec.Includes.Aggregate(Query,(Currentquery,IncludeExpression)
                  =>Currentquery.Include(IncludeExpression));
            //_dbcontext.Products.OredrBy(p=>p.Name).include(p => p.productBrand)
            //_dbcontext.Products.OredrBy(p=>p.Name).include(p => p.productBrand).include(p => p.productType)



            // _dbcontext.Set<T>().Where(p => p.Id == id) currentquery
            //_dbcontext.Set<T>().Where(p=>p.Id == id).Include(p => p.productBrand)
            //_dbcontext.Set<T>().Where(p => p.Id == id).Include(p => p.productBrand).Include(p => p.productType).ToListAsync();
            return Query;
        }
    }
}
