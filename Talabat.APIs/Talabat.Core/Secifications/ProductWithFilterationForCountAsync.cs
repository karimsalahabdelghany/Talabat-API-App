using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Secifications
{
    public class ProductWithFilterationForCountAsync:BaseSpecifications<Product>
    {
       
        public ProductWithFilterationForCountAsync(ProductSpecParams Params) : base(p =>
           (string.IsNullOrEmpty(Params.Search)|| p.Name.Contains(Params.Search))
            &&
           (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId) //True
            &&
           (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)) //True
                 
        {

        }
    }
}
