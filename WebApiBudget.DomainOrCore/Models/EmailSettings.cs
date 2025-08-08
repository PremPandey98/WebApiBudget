namespace WebApiBudget.DomainOrCore.Models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = null!;
        public string SmtpPassword { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public bool EnableSsl { get; set; } = true;
        public int OtpExpirationMinutes { get; set; } = 15;
    }
}