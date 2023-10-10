using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied
{
    public static class GetAllOrchesterMitglied
    {
        public static void MapOrchesterMitgliedGetAllEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/orchesterMitglied/getAll", GetAllOrchesterMitglieder);
                //.RequireAuthorization();
        }

        private static async Task<IResult> GetAllOrchesterMitglieder(ISender sender, CancellationToken cancellationToken)
        {
            var allOrchesterMitglieder = await sender.Send(new GetAllOrchesterMitgliederQuery());
            return Results.Ok(allOrchesterMitglieder);
        }

        private record GetAllOrchesterMitgliederResponse(Guid Id, string Vorname, string Nachname, Adresse Adresse, DateTime Geburtstag, string Telefonnummer, string Handynummer, int? DefaultInstrument, int? DefaultNotenStimme);

        private record GetAllOrchesterMitgliederQuery : IRequest<GetAllOrchesterMitgliederResponse[]> { };

        private class GetAllOrchesterMitgliederQueryHandler : IRequestHandler<GetAllOrchesterMitgliederQuery, GetAllOrchesterMitgliederResponse[]>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetAllOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<GetAllOrchesterMitgliederResponse[]> Handle(GetAllOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                return orchesterMitglieder
                    .Select(o => new GetAllOrchesterMitgliederResponse(o.Id.Value, o.Vorname, o.Nachname, o.Adresse, o.Geburtstag, o.Telefonnummer, o.Handynummer, o.DefaultInstrument, o.DefaultNotenStimme)).ToArray();
            }
        }
    }
}
