namespace WebApiBudget.DomainOrCore.Entities
{
    public class BlacklistedTokenEntity
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
