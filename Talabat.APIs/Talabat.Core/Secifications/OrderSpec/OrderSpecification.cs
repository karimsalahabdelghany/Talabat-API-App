using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Secifications.OrderSpec
{
    public class OrderSpecification : BaseSpecifications<Order>
    {
        public OrderSpecification(string email):base(o=>o.BuyerEmail == email)
        { 
            Includes.Add(D=>D.DeliveryMethod);
            Includes.Add(oi => oi.Items);                  //eager loading
            AddOrderByDescinding(O=>O.OrderDate);
            
        }
        public OrderSpecification(string email,int OrderId):base(O=>O.BuyerEmail==email && O.Id==OrderId)
        {
            Includes.Add(D => D.DeliveryMethod);
            Includes.Add(oi => oi.Items);
        }
           
        
    }
}
