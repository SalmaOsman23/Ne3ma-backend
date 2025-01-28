using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neama.Errors;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neama.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);//move to next middleware

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                //log exception to DB (Notnow)

                //response to frontend .. head of response
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //body of response
                //return exception to frontend
                var exceptionErrorResponse = env.IsDevelopment() ?
                    new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExceptionResponse(500);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(exceptionErrorResponse, options);//convert it to Json 
                await context.Response.WriteAsync(json);
            }

        }

    }
}
