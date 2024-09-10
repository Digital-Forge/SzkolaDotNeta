using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace Web.Middleware
{
    internal class GeneralExceptionMiddleware(IServiceProvider _serviceProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 500; // Bad Request
            Log.Error(exception, "Exception");
            return true;
        }
    }
}