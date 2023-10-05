using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;
using static TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints.CreateOrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class TerminCreate
    {
        public static void MapTerminCreateEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/Termin/create", PostTerminCreate)
                .RequireAuthorization();
        }

        private static async Task<IResult> PostTerminCreate([FromBody] CreateOrchesterMitgliedCommand createOrchesterMitgliedCommand, ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(createOrchesterMitgliedCommand, cancellationToken);

            return Results.Ok(result);
        }

        public record CreateTerminCommand(string Name, int TerminArt, DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, int[] Noten, int[] Uniform, Guid[]? OrchestermitgliedIds) : IRequest<Domain.TerminAggregate.Termin>;

        public class CreateTerminCommandHandler : IRequestHandler<CreateTerminCommand, Domain.TerminAggregate.Termin>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public CreateTerminCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<Domain.TerminAggregate.Termin> Handle(CreateTerminCommand request, CancellationToken cancellationToken)
            {
                Domain.OrchesterMitgliedAggregate.OrchesterMitglied[] orchesterMitglieder;
                if (request.OrchestermitgliedIds is not null && request.OrchestermitgliedIds.Length > 0)
                {
                    orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(request.OrchestermitgliedIds.Select(id => OrchesterMitgliedsId.Create(id)).ToArray(), cancellationToken);
                }
                else
                {
                    orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                }

                var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o => TerminRückmeldungOrchestermitglied.Create(o.Id, new List<int?> { o.DefaultInstrument }, new List<int?> { o.DefaultNotenStimme })).ToArray();

                var treffpunkt = Adresse.Create(request.TreffPunkt.Straße, request.TreffPunkt.Hausnummer, request.TreffPunkt.Postleitzahl, request.TreffPunkt.Stadt);

                var termin = Domain.TerminAggregate.Termin.Create(terminRückmeldungOrchesterMitglieder, request.Name, request.TerminArt, request.StartZeit, request.EndZeit, treffpunkt, request.Noten.ToList(), request.Uniform.ToList());

                return await terminRepository.Save(termin, cancellationToken);
            }
        }
    }
}
