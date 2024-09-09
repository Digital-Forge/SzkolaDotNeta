using Application.Abstract;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Middleware
{
    public class AppSystemExceptionMiddleware(IServiceProvider _serviceProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not AppSystemException) return false;

            var serviceException = exception as AppSystemException;

            switch (serviceException.TypeAction)
            {
                case AppSystemException.ExceptionTypeAction.Error:
                    httpContext.Response.StatusCode = 500; // INTERNAL SERVER ERROR
                    break;
                case AppSystemException.ExceptionTypeAction.Argument:
                    httpContext.Response.StatusCode = 400; // Bad Request
                    break;
                case AppSystemException.ExceptionTypeAction.Unauthoryze:
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    break;
                case AppSystemException.ExceptionTypeAction.NotFound:
                    httpContext.Response.StatusCode = 404; // Not Found
                    break;
                case AppSystemException.ExceptionTypeAction.NotAccess:
                    httpContext.Response.StatusCode = 403; // Forbidden
                    break;
                default:
                    httpContext.Response.StatusCode = 501;
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        await scope.ServiceProvider.GetService<ILogService>().ExceptionLogAsync(new NotImplementedException());
                    }
                    break;
            };
            return true;
        }
    }
}
