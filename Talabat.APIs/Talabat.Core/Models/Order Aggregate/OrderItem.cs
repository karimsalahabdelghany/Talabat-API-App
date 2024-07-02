﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class OrderItem : BaseEntite
    {
        public OrderItem() { }

        public OrderItem(ProductItemOrdered product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public ProductItemOrdered Product { get; set; }
        public int Quantity {  get; set; }
        public decimal Price {  get; set; }
    }
}
