using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Interfaces
{
    public interface IVerificationEmailService
    {
        Task SendTo(User user, string clientUri);
    }
}