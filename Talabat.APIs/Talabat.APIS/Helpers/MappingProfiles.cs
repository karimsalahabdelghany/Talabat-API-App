using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.APIS.Helpers;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Identity;
using IdentityAddress = Talabat.Core.Models.Identity.Address;
using OrderAddress = Talabat.Core.Models.Order_Aggregate.Address;



namespace Talabat.APIS.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.productType, o => o.MapFrom(p => p.productType.Name))
                .ForMember(p => p.productBrand, o => o.MapFrom(p => p.productBrand.Name))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<PictureUrlResolver>());

            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d =>d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductId,o=>o.MapFrom(s=>s.Product.ProductId))
                .ForMember(d=>d.ProductName,o=>o.MapFrom(s=>s.Product.ProductName))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom(s=>s.Product.PictureUrl))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemPictureUrlResolver>());
                
            
        }
    }
}
