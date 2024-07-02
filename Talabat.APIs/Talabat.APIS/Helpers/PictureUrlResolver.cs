using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.Core.Models;

namespace Talabat.APIS.Helpers
{
    public class PictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl)) 
            {
                return $"{_configuration["APIBaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
