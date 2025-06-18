namespace WebApiBudget.Application.Authentication.Models
{
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
