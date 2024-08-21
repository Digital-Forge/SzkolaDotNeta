using Application.Interfaces.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Middleware
{
    internal class GeneralExceptionMiddleware(IServiceProvider _serviceProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500; // Bad Request

            using (var scope = _serviceProvider.CreateScope())
            {
                await scope.ServiceProvider.GetService<ILogService>().ExceptionLogAsync(exception);
            }

            return true;
        }
    }
}