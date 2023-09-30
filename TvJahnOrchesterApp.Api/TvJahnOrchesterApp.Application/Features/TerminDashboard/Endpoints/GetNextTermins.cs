using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Application.Features.OrchesterMitglied;

namespace TvJahnOrchesterApp.Application.Features.TerminDashboard
{
    public static partial class GetNextTermins
    {
        public static void MapGetNextTerminEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Dashboard/NextTermins", GetNextTerminsDashboard)
                .RequireAuthorization();
        }

        public static async Task<TerminOverview[]> GetNextTerminsDashboard(CancellationToken cancellationToken, ISender sender)
        {
            return await sender.Send(new GetNextTerminsQuery());
        }

        public record GetNextTerminsQuery() : IRequest<TerminOverview[]>;

        public class GetNextTerminsQueryHandler : IRequestHandler<GetNextTerminsQuery, TerminOverview[]>
        {
            private const int DAYS_TO_INCLUDE_TERMIN = 14;
            
            private readonly ITerminRepository terminRepository;
            private readonly ICurrentUserService currentUserService;

            public GetNextTerminsQueryHandler(ITerminRepository terminRepository, ICurrentUserService currentUserService)
            {
                this.terminRepository = terminRepository;
                this.currentUserService = currentUserService;
            }

            public async Task<TerminOverview[]> Handle(GetNextTerminsQuery request, CancellationToken cancellationToken)
            {
                var terminsInNextDays = (await terminRepository.GetAll(cancellationToken)).Where(IsInDayRanch);
                var currentOrchesterMember = await currentUserService.GetCurrentOrchesterMitgliedAsync(cancellationToken);
                var terminsForCurrentUser = terminsInNextDays.Where(t => t.IstZugeordnet(currentOrchesterMember.Id));
                return terminsForCurrentUser.Select(x => new TerminOverview(x.Id.Value, x.Name, x.TerminArt, x.EinsatzPlan.StartZeit)).ToArray();
            }

            private bool IsInDayRanch(Domain.TerminAggregate.Termin termin)
            {
                var timespan = termin.EinsatzPlan.StartZeit - DateTime.Now;
                return 0 <= timespan.Days && timespan.Days <= DAYS_TO_INCLUDE_TERMIN;
            }
        }
    }
}
