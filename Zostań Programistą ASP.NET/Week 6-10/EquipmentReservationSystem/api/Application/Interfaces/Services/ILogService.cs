using Application.Abstract;

namespace Application.Interfaces.Services
{
    public interface ILogService
    {
        void InfoLog(string name, string message = null, string source = null);
        Task InfoLogAsync(string name, string message = null, string source = null);
        void ExceptionLog(Exception exception, long? parentLogId = null);
        Task ExceptionLogAsync(Exception exception, long? parentLogId = null);
        void SystemExceptionLog(AppSystemException exception);
        Task SystemExceptionLogAsync(AppSystemException exception);
    }
}
