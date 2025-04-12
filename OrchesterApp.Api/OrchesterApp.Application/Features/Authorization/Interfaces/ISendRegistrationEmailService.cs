namespace TvJahnOrchesterApp.Application.Features.Authorization.Interfaces
{
    public interface ISendRegistrationEmailService
    {
        Task SendTo(string email, string registrationKey, string clientUri);
    }
}
