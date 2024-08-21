using Domain.Interfaces.Repositories;
using Domain.Models.System;
using Infrastructure.Attributes;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    [AutoRegisterScopedRepository(typeof(ILogRepository))]
    public class LogRepository(Context _context) : ILogRepository
    {
        public long AddLog(Log log)
        {
            log.Date = DateTime.Now;
            log.UserId = _context.GetContextUser()?.Id;

            _context.Logs.Add(log);
            _context.SaveChanges();

            TerminalLog(log);
            return log.Id;
        }

        public async Task<long> AddLogAsync(Log log)
        {
            log.Date = DateTime.Now;
            log.UserId = _context.GetContextUser()?.Id;

            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();

            TerminalLog(log);
            return log.Id;
        }

        private void TerminalLog(Log log)
        {
            Console.WriteLine(log);
        }
    }
}
