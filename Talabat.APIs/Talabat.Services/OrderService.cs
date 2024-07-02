using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Repositiries;
using Talabat.Core.Secifications.OrderSpec;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        
        private readonly IbasketRepository _basketrepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IbasketRepository basketRepo,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            
            _basketrepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket =await _basketrepo.GetBasketAsync(basketId);

            //2.Get Selected Items at Basket From Product Repo
            var orderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product =await _unitOfWork.Repository<Product>().GetbyIdAsync(item.Id);
                    var ProductItemOdered = new ProductItemOrdered(Product.Id,Product.Name,Product.PictureUrl);
                    var orderItem = new OrderItem(ProductItemOdered,item.Quantity,Product.Price);
                    orderItems.Add(orderItem);
                }
            }

            //3.Calculate SubTotal   //price of product * quantity
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity); //loop on orderItems
                
            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetbyIdAsync(DeliveryMethodId);
            //5.Create Order
            var spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var Exorder =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(Exorder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(Exorder);
               await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethod, orderItems, subtotal,Basket.PaymentIntentId);
            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7.Save Order To Database
           var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
            var Order =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec); 
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var Orders =await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods =await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }
    }
}
