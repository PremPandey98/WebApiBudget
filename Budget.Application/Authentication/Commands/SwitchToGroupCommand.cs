using MediatR;
using WebApiBudget.Application.Authentication.Interfaces;
using WebApiBudget.Application.Authentication.Models;


namespace WebApiBudget.Application.Authentication.Commands
{
    public record SwitchToGroupCommand(Guid UserId, Guid GroupId) : IRequest<AuthResponse>;
    public class SwitchToGroupCommandHandler(IAuthService authService) : IRequestHandler<SwitchToGroupCommand, AuthResponse?>
    {
        public async Task<AuthResponse?> Handle(SwitchToGroupCommand request, CancellationToken cancellationToken)
        {
            var Group = await authService.GetGroupAsync(request.GroupId);
            if (Group == null)
                return null;

            var user = await authService.GetUserAsync(request.UserId);
            if (user == null)
                return null;
            if(user.Groups.Contains(Group))
            {
                var token = await authService.GenerateTokenAsync(user);

                return new AuthResponse
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    GroupId = Group.Id,
                    SwitchToGroup = true,
                    Token = token
                };
            }
            return null;

            


        }
    }
}
