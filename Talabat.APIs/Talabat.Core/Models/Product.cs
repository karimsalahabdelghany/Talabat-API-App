using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models
{
    public class Product:BaseEntite
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public  decimal Price { get; set; }
        public  int ProductBrandId {  get; set; } //FK from ProductBrandTable
        public ProductBrand productBrand { get; set; }
        public int ProductTypeId {  get; set; }   //FK
        public ProductType productType { get; set; }
    }
}
