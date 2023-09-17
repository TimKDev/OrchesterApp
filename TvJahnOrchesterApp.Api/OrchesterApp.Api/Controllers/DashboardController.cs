using BuberDinner.Api.Controllers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Contracts.Dashboard;

namespace TvJahnOrchesterApp.Api.Controllers
{
    public class DashboardController : ApiController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public DashboardController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpGet("NextTermins")]
        public async Task<IActionResult> GetNextTermins(CancellationToken cancellationToken)
        {
            return Ok(new GetNextTerminResponse(new Contracts.Dashboard.Dto.TerminOverviewDto[] {}));
        }

        [HttpGet("NotRepliedTermins")]
        public async Task<IActionResult> GetNotRepliedTermins(CancellationToken cancellationToken)
        {
            return Ok(new GetNotRepliedTerminResponse(new Contracts.Dashboard.Dto.TerminOverviewDto[] { }));
        }
    }
}
