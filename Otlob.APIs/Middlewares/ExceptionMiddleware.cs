using Microsoft.EntityFrameworkCore.ChangeTracking;
using Otlob.APIs.Errors;
using System.Net;
using System.Text.Json;

namespace Otlob.APIs.Middlewares
{


    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate _Next,ILogger<ExceptionMiddleware> _logger,IHostEnvironment _env)
        {
            next = _Next;
            logger = _logger;
            env = _env;
        }
        public async Task InvokeAsync(HttpContext context ) 
        {
            try 
            {
                await next.Invoke(context);
            }catch (Exception e)
            {
                logger.LogError(e, e.Message);
                //production log ex in db
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError ;

                //if (env.IsDevelopment())
                //{
                //    var Response = new ApiExceptionResponse(500, e.Message, e.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                //}


                var Response = env.IsDevelopment() ? new ApiExceptionResponse(500, e.Message, e.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(Response, Options);
                context.Response.WriteAsync(JsonResponse);
            }
        }

    }
}
