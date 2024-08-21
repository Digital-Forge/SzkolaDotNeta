namespace Domain.Models.Business
{
    public class ServiceNote : AuditableEntity
    {
        public string Note { get; set; }
        public Guid ItemInstanceId { get; set; }
    }
}
