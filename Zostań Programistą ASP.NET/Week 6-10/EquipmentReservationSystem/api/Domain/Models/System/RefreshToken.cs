namespace Domain.Models.System
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime LifeTime { get; set; }
        public Guid UserId { get; set; }
    }
}
