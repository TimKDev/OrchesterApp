using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;

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

        public record GetAllOrchesterMitgliederQuery : IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> { };

        public class GetAllOrchesterMitgliederQueryHandler : IRequestHandler<GetAllOrchesterMitgliederQuery, Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]>
        {
            private readonly IOrchesterMitgliedRepository orchesterMitgliedRepository;

            public GetAllOrchesterMitgliederQueryHandler(IOrchesterMitgliedRepository orchesterMitgliedRepository)
            {
                this.orchesterMitgliedRepository = orchesterMitgliedRepository;
            }

            public async Task<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> Handle(GetAllOrchesterMitgliederQuery request, CancellationToken cancellationToken)
            {
                var orchesterMitglieder = await orchesterMitgliedRepository.GetAllAsync(cancellationToken);
                return orchesterMitglieder;
            }
        }
    }
}
