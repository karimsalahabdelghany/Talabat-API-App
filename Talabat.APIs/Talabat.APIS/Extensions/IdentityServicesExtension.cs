using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;
using Talabat.Repositiory.Identity;
using Talabat.Services;

namespace Talabat.APIS.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddIdentity<AppUser,IdentityRole>()       //configure services for interfaces
               .AddEntityFrameworkStores<AppIdentityDbContext>();      //configure services for classes that implment that interfaces

            Services.AddAuthentication(options=>
            {
                options.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;
            }
            
            )// usermanager- signinManager - roleManager -
                                                                              // to specify schema of token
                 .AddJwtBearer(options =>
                      options.TokenValidationParameters = new TokenValidationParameters()
                      {
                          ValidateIssuer = true,
                          ValidIssuer = configuration["JWT:ValidIssuer"],
                          ValidateAudience = true,
                          ValidAudience = configuration["JWT:ValidAudience"],
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                      

        }
                 );

            return Services;

        }
    }
}
