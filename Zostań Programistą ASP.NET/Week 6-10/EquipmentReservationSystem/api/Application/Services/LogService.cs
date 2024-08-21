using Application.Abstract;
using Application.Attributes;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Domain.Models.System;
using Domain.Utils;

namespace Application.Services
{
    [AutoRegisterScopedService(typeof(ILogService))]
    public class LogService(ILogRepository _logRepository) : ILogService
    {
        public void InfoLog(string name, string message = null, string source = null)
        {
            var log = new Log()
            {
                Name = name,
                Message = message,
                Source = source,
                Type = LogType.Info,
            };

            _logRepository.AddLog(log);
        }

        public async Task InfoLogAsync(string name, string message = null, string source = null)
        {
            var log = new Log()
            {
                Name = name,
                Message = message,
                Source = source,
                Type = LogType.Info,
            };

            await _logRepository.AddLogAsync(log);
        }

        public void ExceptionLog(Exception exception, long? parentLogId = null)
        {
            if (exception == null) return;

            var log = new Log()
            {
                Name = exception.GetType().FullName,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                Type = LogType.Exception,
                ParentLogId = parentLogId
            };

            var logId = _logRepository.AddLog(log);

            if (exception is AggregateException aggregateException) 
            {
                foreach (var except in aggregateException.InnerExceptions)
                {
                    ExceptionLog(except, logId);
                }
            }
            else if (exception.InnerException != null) ExceptionLog(exception.InnerException, logId);            
        }

        public async Task ExceptionLogAsync(Exception exception, long? parentLogId = null)
        {
            if (exception == null) return;

            var log = new Log()
            {
                Name = exception.GetType().FullName,
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                Type = LogType.Exception,
                ParentLogId = parentLogId
            };

            var logId = await _logRepository.AddLogAsync(log);

            if (exception is AggregateException aggregateException)
            {
                foreach (var except in aggregateException.InnerExceptions)
                {
                    await ExceptionLogAsync(except, logId);
                }
            }
            else if (exception.InnerException != null) await ExceptionLogAsync(exception.InnerException, logId);
        }

        public void SystemExceptionLog(ServiceException exception)
        {
            if (exception == null) return;

            var log = new Log()
            {
                Name = exception.GetType().Name,
                Message = $"Description: \"{exception.Description}\"; Message: \"{exception.Message}\"",
                Source = $"System: \"{exception.Occurred}\", Action: \"{exception.TypeAction}\"; Source: \"{exception.Source}\"",
                StackTrace = exception.StackTrace,
                Type = LogType.SystemException,
            };

            var logId = _logRepository.AddLog(log);

            if (exception.InnerException != null) ExceptionLog(exception.InnerException, logId);
        }

        public async Task SystemExceptionLogAsync(ServiceException exception)
        {
            if (exception == null) return;

            var log = new Log()
            {
                Name = exception.GetType().Name,
                Message = $"Description: \"{exception.Description}\"; Message: \"{exception.Message}\"",
                Source = $"System: \"{exception.Occurred}\", Action: \"{exception.TypeAction}\"; Source: \"{exception.Source}\"",
                StackTrace = exception.StackTrace,
                Type = LogType.SystemException,
            };

            var logId = await _logRepository.AddLogAsync(log);

            if (exception.InnerException != null) await ExceptionLogAsync(exception.InnerException, logId);
        }
    }
}
