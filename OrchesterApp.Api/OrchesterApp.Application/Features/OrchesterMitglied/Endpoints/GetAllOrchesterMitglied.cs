using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Common.Services;

namespace TvJahnOrchesterApp.Application.Features.OrchesterMitglied.Endpoints
{
    public static class GetAllOrchesterMitglied
    {
        public static void MapOrchesterMitgliedGetAllEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/orchester-mitglied/get-all", GetAllOrchesterMitglieder)
                .RequireAuthorization();
        }

        private static async Task<IResult> GetAllOrchesterMitglieder(ISender sender, CancellationToken cancellationToken)
        {
            var allOrchesterMitglieder = await sender.Send(new GetAllOrchesterMitgliederQuery());
            return Results.Ok(allOrchesterMitglieder);
        }

        private record GetAllOrchesterMitgliederResponse(Guid Id, string Vorname, string Nachname, string? Image, string? DefaultInstrument, int? MemberSinceInYears);

        private record GetAllOrchesterMitgliederQuery : IRequest<GetAllOrchesterMitgliederResponse[]> { };

        private class GetAllOrchesterMitgliederQueryHandler : IRequestHandler<GetAllOrchesterMitgliederQuery, GetAllOrchesterMitgliederResponse[]>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;
            private readonly IInstrumentRepository instrumentRepository;

            public GetAllOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository, IInstrumentRepository instrumentRepository)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
                this.instrumentRepository = instrumentRepository;
            }

            public async Task<GetAllOrchesterMitgliederResponse[]> Handle(GetAllOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var result = new List<GetAllOrchesterMitgliederResponse>();
                var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                foreach (var mitglied in orchesterMitglieder)
                {
                    Instrument? instrument = null;
                    if (mitglied.DefaultInstrument is not null)
                    {
                        instrument = await instrumentRepository.GetByIdAsync((int)mitglied.DefaultInstrument, cancellationToken);
                    }

                    var imageAsBase64 = TransformImageService.ConvertByteArrayToBase64(mitglied.Image);

                    result.Add(new GetAllOrchesterMitgliederResponse(mitglied.Id.Value, mitglied.Vorname, mitglied.Nachname, imageAsBase64, instrument?.Value, mitglied.MemberSinceInYears));
                   
                }
                return result.OrderBy(e => e.Vorname).ToArray();
            }
        }
    }
}
