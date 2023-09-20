using MediatR;
using Microsoft.AspNetCore.Identity;
using TvJahnOrchesterApp.Application.Authentication.Common;
using TvJahnOrchesterApp.Application.Authentication.Common.Errors;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly UserManager<User> userManager;
        private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;

        public RegisterCommandHandler(
            UserManager<User> userManager, 
            IOrchesterMitgliedRepository orchesterMitgliedRepository, 
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var orchesterMitglied = await orchesterMitgliedRepository.GetByRegistrationKeyAsync(Domain.OrchesterMitgliedAggregate.OrchesterMitglied.GetHashString(request.RegisterationKey), cancellationToken);
            if(orchesterMitglied is null)
            {
                throw new InvalidRegistrationKeyException();
            }
            //TTODO
            //Vermischen sich hier nicht zwei Kontexte von unterschiedlichen Aggregaten??? => Muss ich doch Events verwenden? Oder sollte ich den User garnicht mehr als Agggregat betrachten? Ein Orchestermitglied kann ja auch an Abstimmungen teilnehmen, dafür braucht man nicht unbendingt eine Userentittät! ....
            if (orchesterMitglied.ValidateRegistrationKey(request.RegisterationKey))
            {
                throw new InvalidRegistrationKeyException();
            }

            var userInfo = User.Create(orchesterMitglied.Id, request.Email);
            userInfo.UserName = request.Email;
            var result = await userManager.CreateAsync(userInfo, request.Password);
            if (!result.Succeeded)
            {
                throw new IdentityRegistrationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            //Irgendwie muss hier noch die ConnectedUserId auf dem Orchestermitgliedsobjekt gesetzt werden 
            var createdUser = await userManager.FindByEmailAsync(request.Email);
            orchesterMitglied.ConnectWithUser(createdUser!.Id);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Erstelle ein Token um diesen User einzuloggen 
            var token = await tokenService.GenerateAccessTokenAsync(createdUser);

            return new AuthenticationResult(createdUser, token);

        }
    }
}
