using Application.Abstract;
using Microsoft.AspNetCore.Diagnostics;

namespace Web.Middleware
{
    public class ServiceExceptionMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ServiceException) return false;

            var serviceException = exception as ServiceException;

            switch (serviceException.TypeAction)
            {
                case ServiceException.ExceptionTypeAction.Error:
                    httpContext.Response.StatusCode = 500; // INTERNAL SERVER ERROR
                    break;
                case ServiceException.ExceptionTypeAction.Argument:
                    httpContext.Response.StatusCode = 400; // Bad Request
                    break;
                case ServiceException.ExceptionTypeAction.Unauthoryze:
                    httpContext.Response.StatusCode = 401; // Unauthorized
                    break;
                case ServiceException.ExceptionTypeAction.NotFound:
                    httpContext.Response.StatusCode = 404; // Not Found
                    break;
                case ServiceException.ExceptionTypeAction.NotAccess:
                    httpContext.Response.StatusCode = 403; // Forbidden
                    break;
                default:
                    throw new NotImplementedException();
            }

            await httpContext.Response.WriteAsync(serviceException.Message);
            return true;
        }
    }
}
