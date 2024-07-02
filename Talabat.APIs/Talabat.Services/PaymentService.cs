using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.TestHelpers.Treasury;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Repositiries;
using Talabat.Core.Secifications.OrderSpec;
using Talabat.Core.Services;
using Product = Talabat.Core.Models.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IbasketRepository _basketRepo;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IbasketRepository basketRepo,IConfiguration configuration,
            IUnitOfWork unitOfWork) 
        {
            _basketRepo = basketRepo;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            //SecretKet
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];
            //Get Basket
            var Basket =await _basketRepo.GetBasketAsync(BasketId);
            if (Basket == null) return null;
            var ShippingPrice = 0M;
            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetbyIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            //Total = subtotal + DM.Cost
            if(Basket.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product =await _unitOfWork.Repository<Product>().GetbyIdAsync(item.Id);
                    if(item.Price != Product.Price )
                    {
                        item.Price = Product.Price;
                    }
                }
            }
            //Subtotal
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);
            //Craete payment intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            //create
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
                    Currency ="usd",
                    PaymentMethodTypes =new List<string>() { "card"}
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            //Update
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)ShippingPrice * 100,
                };

               paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId,Options);
               Basket.PaymentIntentId = paymentIntent.Id;
               Basket.ClientSecret = paymentIntent.ClientSecret;

            }
           await _basketRepo.UpdateBasketAsync(Basket);
           return Basket;  
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentintenid, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(paymentintenid);
            var Order =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(flag)
            {
               Order.Status = OrderStatus.PaymentRecived;
            }
            else
            {
                Order.Status =OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(Order);
            await _unitOfWork.CompleteAsync();
            return Order;
        }
    }
}
