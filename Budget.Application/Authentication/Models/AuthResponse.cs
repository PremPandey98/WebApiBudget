namespace WebApiBudget.Application.Authentication.Models
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public bool SwitchToGroup { get; set; } = false;
        public Guid GroupId { get; set; }
    }
}
