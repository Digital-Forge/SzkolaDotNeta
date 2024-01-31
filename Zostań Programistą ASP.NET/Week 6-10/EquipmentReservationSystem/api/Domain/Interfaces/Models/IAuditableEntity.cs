using Domain.Utils;

namespace Domain.Interfaces.Models
{
    public interface IAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public EntityStatus? EntityStatus { get; set; }
        public bool Active { get; set; }
    }
}
