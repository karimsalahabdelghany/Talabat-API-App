﻿using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.APIS.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }     //Name
        public decimal DeliveryMethodCost { get; set; }     //Cost
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } 
    }

    
}
