using Domain.Models.System;

namespace Domain.Interfaces.Repositories
{
    public interface ILogRepository
    {
        long AddLog(Log log);
        Task<long> AddLogAsync(Log log);
    }
}
