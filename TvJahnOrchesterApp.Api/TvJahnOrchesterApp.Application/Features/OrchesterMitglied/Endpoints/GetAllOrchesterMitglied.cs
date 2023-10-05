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
            app.MapGet("api/OrchesterMitglied/GetAll", GetAllOrchesterMitglieder)
                .RequireAuthorization();
        }

        public static async Task<IResult> GetAllOrchesterMitglieder(CancellationToken cancellationToken, ISender sender)
        {
            var allOrchesterMitglieder = await sender.Send(new GetAllOrchesterMitgliederQuery());
            return Results.Ok(allOrchesterMitglieder);
        }

        public record GetAllOrchesterMitgliederResponse(Guid id, string vorname, string nachname, Adresse adresse, DateTime geburtstag, string telefonnummer, string handynummer, int defaultInstrument, int defaultNotenStimme);

        public record GetAllOrchesterMitgliederQuery : IRequest<GetAllOrchesterMitgliederResponse[]> { };

        public class GetAllOrchesterMitgliederQueryHandler : IRequestHandler<GetAllOrchesterMitgliederQuery, GetAllOrchesterMitgliederResponse[]>
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
                    .Select(o => new GetAllOrchesterMitgliederResponse(o.Id.Value, o.Vorname, o.Nachname, o.Adresse, o.Geburtstag, o.Telefonnummer, o.Handynummer, o.DefaultInstrument.Value, o.DefaultNotenStimme.Value)).ToArray();
            }
        }
    }
}
