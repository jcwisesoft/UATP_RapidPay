using Code_Test_UATP_RapidPay.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Code_Test_UATP_RapidPay.Infrastructure
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment hostingEnvironment)
        {

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        if (contextFeature.Error.GetType() == typeof(InvalidOperationException) ||
                            contextFeature.Error.GetType() == typeof(ArgumentException)
                             )

                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                            await context.Response.WriteAsync(new Result
                            {
                                Status = HttpStatusCode.BadRequest,
                                Message = contextFeature.Error.Message,
                            }.ToString());

                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                            await context.Response.WriteAsync(new Result
                            {
                                Status = HttpStatusCode.InternalServerError,
                                //message = hostingEnvironment.IsProduction() ? "We currently cannot complete this request process. Please retry or contact our agent support network" : contextFeature.Error.Message
                                Message = contextFeature.Error.Message,
                            }.ToString());
                        }
                    }
                });
            });
        }
    }
}
