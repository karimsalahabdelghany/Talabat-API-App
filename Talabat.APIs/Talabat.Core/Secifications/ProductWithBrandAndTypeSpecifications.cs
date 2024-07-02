using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Secifications
{
    public class ProductWithBrandAndTypeSpecifications:BaseSpecifications<Product>
    {
        //ctor is used for get all products
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params)
            :base(p=>
                   (string.IsNullOrEmpty(Params.search) || p.Name.Contains(Params.search))
                    &&
                   (!Params.BrandId.HasValue || p.ProductBrandId==Params.BrandId) //True
                   &&
                    (!Params.TypeId.HasValue || p.ProductTypeId ==Params.TypeId) //True
                 )
        {
            
            Includes.Add(p => p.productType);
            Includes.Add(p => p.productBrand);
            if(!string.IsNullOrEmpty(Params.Sort))
            {
                switch(Params.Sort)
                {
                    case "PriceAsc":
                        AddOredrBy(p=>p.Price);
                            break;
                    case "PriceDesc":
                        AddOrderByDescinding(p => p.Price);
                        break;
                    default: 
                        AddOredrBy(p=>p.Name);
                        break;

                }
            }

            ApplyPageination(Params.PageSize*(Params.PageIndex-1), Params.PageSize);
          
        }
        //ctor is used for get product by id
        public ProductWithBrandAndTypeSpecifications(int id):base(p=>p.Id==id)
        {
           
            Includes.Add(p => p.productType);
            Includes.Add(p => p.productBrand);
        }
        
    }
}
