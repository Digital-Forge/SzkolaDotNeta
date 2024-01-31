namespace Domain.Models.System
{
    public class DataFile : AuditableEntity
    {
        public string OriginName { get; set; }
        public string Format { get; set; }
    }
}
