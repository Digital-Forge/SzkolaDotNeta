using Domain.Utils;

namespace Domain.Models.System
{
    public class Log
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
        public string? StackTrace { get; set; }
        public LogType Type { get; set; }
        public DateTime Date { get; set; }
        public Guid? UserId { get; set; }

        public long? ParentLogId { get; set; }
    }
}
