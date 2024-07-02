using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.APIS.Helper;
using Talabat.Core;
using Talabat.Core.Repositiries;
using Talabat.Core.Services;
using Talabat.Repositiory;
using Talabat.Services;

namespace Talabat.APIS.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
           //Services.AddScoped(typeof(IGenricRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped(typeof(IbasketRepository),typeof(BasketRepositiory));
            Services.AddScoped<IOrderService,OrderService>();
            //builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));
            #region Error-Handling
            Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    //Modelstate =>Dic [keyvaluepair]
                    //key =>name of parameter
                    //value => Error
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(p => p.ErrorMessage)
                                                        .ToArray();

                    var validationerrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationerrorResponse);
                };

            });
            return Services;        
            #endregion
        }
    }
}
