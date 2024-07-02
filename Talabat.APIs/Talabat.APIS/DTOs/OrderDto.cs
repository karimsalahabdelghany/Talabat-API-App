using System.ComponentModel.DataAnnotations;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.APIS.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        
        public int DeliveryMethodId { get; set; }
       
        public AddressDto ShippingAddress { get; set; }
    }
}
