using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Neama.Core.Services;
using Neama.Errors;
using Neama.Services;
using System.Linq;

namespace Neama.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            //services.AddIdentityServices();
            services.AddScoped<ITokenService, TokenServices>();

            //this for validation errors since in validation error will not enter to endpoint so i have to handle it in startup
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count() > 0)
                                                             .SelectMany(M => M.Value.Errors)
                                                             .Select(E => E.ErrorMessage)
                                                             .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            return services;
        }
    }
}
