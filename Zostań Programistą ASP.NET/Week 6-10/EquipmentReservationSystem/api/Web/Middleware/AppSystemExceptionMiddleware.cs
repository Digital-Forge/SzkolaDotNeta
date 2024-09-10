using Application.Abstract;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace Web.Middleware
{
    public class AppSystemExceptionMiddleware(IServiceProvider _serviceProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not AppSystemException) return false;

            var appSystemException = exception as AppSystemException;

            switch (appSystemException.TypeAction)
            {
                case AppSystemException.ExceptionTypeAction.Error:
                    httpContext.Response.StatusCode = 500; // INTERNAL SERVER ERROR
                    Log.Error(appSystemException, $"{appSystemException.Occurred} - {appSystemException.GetType().Name}");
                    break;
                case AppSystemException.ExceptionTypeAction.Argument:
                    httpContext.Response.StatusCode = 400; // Bad Request
                    Log.Information(appSystemException, $"{appSystemException.Occurred} - {appSystemException.GetType().Name}");
                    break;
                case AppSystemException.ExceptionTypeAction.Unauthoryze:
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    Log.Information(appSystemException, $"{appSystemException.Occurred} - {appSystemException.GetType().Name}");
                    break;
                case AppSystemException.ExceptionTypeAction.NotFound:
                    httpContext.Response.StatusCode = 404; // Not Found
                    Log.Information(appSystemException, $"{appSystemException.Occurred} - {appSystemException.GetType().Name}");
                    break;
                case AppSystemException.ExceptionTypeAction.NotAccess:
                    httpContext.Response.StatusCode = 403; // Forbidden
                    Log.Warning(appSystemException, $"{appSystemException.Occurred} - {appSystemException.GetType().Name}");
                    break;
                default:
                    httpContext.Response.StatusCode = 501;
                    Log.Fatal(appSystemException, $"{appSystemException.Occurred + " - "}AppSystemException.ExceptionTypeAction type not handled");
                    break;
            };
            return true;
        }
    }
}
