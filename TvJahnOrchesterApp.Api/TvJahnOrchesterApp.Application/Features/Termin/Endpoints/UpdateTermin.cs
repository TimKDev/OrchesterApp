using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Domain.OrchesterMitgliedAggregate.ValueObjects;
using TvJahnOrchesterApp.Domain.TerminAggregate.Entities;

namespace TvJahnOrchesterApp.Application.Features.Termin.Endpoints
{
    public static class UpdateTermin
    {
        public static void MapUpdateTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/Termin/getById/{id}", GetTerminById)
                .RequireAuthorization();
        }

        public static async Task<IResult> GetTerminById(UpdateTerminCommand updateTerminCommand, ISender sender, CancellationToken cancellationToken)
        {
            var response = await sender.Send(updateTerminCommand, cancellationToken);
            return Results.Ok(response);
        }

        public record UpdateTerminCommand(Guid TerminId, string Name, int TerminArt, Guid[]? OrchestermitgliedIds) : IRequest<Domain.TerminAggregate.Termin>;

        public class UpdateTerminCommandHandler : IRequestHandler<UpdateTerminCommand, Domain.TerminAggregate.Termin>
        {
            private readonly ITerminRepository terminRepository;
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IUnitOfWork unitOfWork;

            public UpdateTerminCommandHandler(ITerminRepository terminRepository, IOrchesterMitgliedRepository orchesterMitgliedRepository, IUnitOfWork unitOfWork)
            {
                this.terminRepository = terminRepository;
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<Domain.TerminAggregate.Termin> Handle(UpdateTerminCommand request, CancellationToken cancellationToken)
            {
                var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
                //TTODO Throw Exception when Termin is not found!
                termin.UpdateName(request.Name);
                termin.UpdateTerminArt(request.TerminArt);

                if (request.OrchestermitgliedIds is null) return termin;

                var orchesterMitglieder = await orchesterMitgliedRepository.QueryByIdAsync(request.OrchestermitgliedIds.Select(OrchesterMitgliedsId.Create).ToArray(), cancellationToken);

                var terminRückmeldungOrchesterMitglieder = orchesterMitglieder.Select(o => TerminRückmeldungOrchestermitglied.Create(o.Id, new List<int?> { o.DefaultInstrument }, new List<int?> { o.DefaultNotenStimme })).ToArray();

                termin.UpdateTerminRückmeldungOrchestermitglied(terminRückmeldungOrchesterMitglieder);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return termin;
            }
        }
    }
}
