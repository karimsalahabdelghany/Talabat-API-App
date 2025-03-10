﻿using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.APIS.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}{source.Product.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
