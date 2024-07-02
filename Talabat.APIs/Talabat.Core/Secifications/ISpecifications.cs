using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Secifications
{
    public interface ISpecifications<T> where T : BaseEntite
    {
        //_dbcontext.Products.Where(p=>p.Id == id).Include(p => p.productBrand).Include(p => p.productType)
        //sign for property for where condition [Where(p=>p.Id == id)]
        public Expression<Func<T,bool>> Criteria {  get; set; }

        //sign for property for list of Include [Include(p => p.productBrand).Include(p => p.productType)]
        public List<Expression<Func<T,object>>> Includes { get; set; }

        // sign for prop for orderby [OredrBy(p=>p.Name)]
        public Expression<Func<T,object>> OrderBy { get; set; }

        //sign for property for orderbydec [OredrByDec(p=>p.Name)]
        public Expression<Func<T,object>> OrderByDecsending { get; set; }

        //Skip(2)
        public int Skip {  get; set; }

        //Take(2)
        public int Take {  get; set; }

        public bool IsPageniationEnabled {  get; set; }
    }
}
