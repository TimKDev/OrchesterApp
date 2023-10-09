using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    //Orchestermitglied sollte nur löschbar sein, wenn es noch nie zu einem Termin oder sonstigem Objekt zugeordnet wurde. Ansonsten soll einfach der Status des Mitglieds auf inaktiv gesetzt werden, der zugehörige User gelöscht und kein neuer Registireungskey erstellt werden. Dann können die anderen Teile der App solche Mitglieder ausblenden bzw. verbieten, dass die betreffenden Mitglieder an Abstimmungen teilnehmen.
    public static class DeleteOrchesterMitglied
    {
        public static void MapDeleteOrchesterMitgliedEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/orchesterMitglied", DeleteDeleteOrchesterMitglied)
                .RequireAuthorization();
        }

        private static async Task<IResult> DeleteDeleteOrchesterMitglied([FromBody] DeleteOrchesterMitgliedCommand deleteOrchesterMitgliedCommand, CancellationToken cancellationToken, ISender sender)
        {
            var message = await sender.Send(deleteOrchesterMitgliedCommand);
            return Results.Ok(message);
        }

        private record DeleteOrchesterMitgliedCommand(Guid OrchesterMitgliedsId) : IRequest<string>;

        private class DeleteOrchesterMitgliedCommandHandler : IRequestHandler<DeleteOrchesterMitgliedCommand, string>
        {
            private IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly UserManager<User> userManager;

            public DeleteOrchesterMitgliedCommandHandler(UserManager<User> userManager, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.userManager = userManager;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<string> Handle(DeleteOrchesterMitgliedCommand request, CancellationToken cancellationToken)
            {
                var orchesterMitgliedsId = OrchesterMitgliedsId.Create(request.OrchesterMitgliedsId);
                var orchesterMitglied = await orchesterMitgliedRepository.GetByIdAsync(orchesterMitgliedsId, cancellationToken);
                if (orchesterMitglied.ConnectedUserId is not null)
                {
                    var user = await userManager.FindByIdAsync(orchesterMitglied.ConnectedUserId) ?? throw new Exception("User not found");
                    await userManager.DeleteAsync(user);
                }
                await orchesterMitgliedRepository.DeleteByIdAsync(orchesterMitgliedsId, cancellationToken);
                
                return "Orchestermitglied wurde vollständig gelöscht.";

            }
        }
    }
}
