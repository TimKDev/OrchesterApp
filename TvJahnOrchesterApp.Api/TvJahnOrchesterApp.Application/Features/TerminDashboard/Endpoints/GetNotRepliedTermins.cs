using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using static TvJahnOrchesterApp.Application.Features.TerminDashboard.GetNextTermins;

namespace TvJahnOrchesterApp.Application.Features.TerminDashboard.Endpoints
{
    public static class GetNotRepliedTermins
    {
        public static void MapNotRepliedTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Dashboard/NotReplied", GetNotRepliedTerminDashboard)
                .RequireAuthorization();
        }

        public static async Task<TerminOverview[]> GetNotRepliedTerminDashboard(CancellationToken cancellationToken, ISender sender)
        {
            return await sender.Send(new GetNotRepliedTerminsQuery());
        }

        public record GetNotRepliedTerminsQuery() : IRequest<TerminOverview[]>;

        public class GetNotRepliedTerminsQueryHandler : IRequestHandler<GetNotRepliedTerminsQuery, TerminOverview[]>
        {
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetNotRepliedTerminsQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<TerminOverview[]> Handle(GetNotRepliedTerminsQuery request, CancellationToken cancellationToken)
            {
                var terminsInFuture = (await terminRepository.GetAll(cancellationToken)).Where(t =>
                (t.EinsatzPlan.StartZeit - DateTime.Now).Days >= 0);

                var currentOrchesterMember = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);

                return terminsInFuture
                        .Where(t => t.IstZugeordnet(currentOrchesterMember.Id))
                        .Where(t => t.NichtZurückgemeldet(currentOrchesterMember.Id))
                        .Select(x => new TerminOverview(x.Id.Value, x.Name, x.TerminArt, x.EinsatzPlan.StartZeit)).ToArray();
            }
        }
    }


}
