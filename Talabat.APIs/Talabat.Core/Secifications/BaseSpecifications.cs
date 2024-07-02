using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Secifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntite
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }= new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDecsending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPageniationEnabled { get; set; }

        //get all
        public BaseSpecifications()
        {
           //Includes = new List<Expression<Func<T, object>>>();

        }
        //get by id
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            // Includes = new List<Expression<Func<T, object>>>();
        }
        //function to set orderby
        public void AddOredrBy(Expression<Func<T,object>> oredrByexpression) 
        { 
            OrderBy = oredrByexpression;
        }
        //function to set orderbyDescding
        public void AddOrderByDescinding(Expression<Func<T,object>>orderbyDescexpression)
        {
            OrderByDecsending = orderbyDescexpression;
        }
        public void ApplyPageination(int skip,int take)
        {
            IsPageniationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }

}
