using Microsoft.AspNetCore.Diagnostics;

namespace Web.Middleware
{
    internal class GeneralExceptionMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = 400; // Bad Request
            await httpContext.Response.WriteAsync(exception.Message);
            return true;
        }
    }
}
