namespace TvJahnOrchesterApp.Application.Features.Authorization.Interfaces
{
    public interface ISendPasswordResetEmailService
    {
        Task SendTo(string email, string resetToken, string clientUri);
    }
} 