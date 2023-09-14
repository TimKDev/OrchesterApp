using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Queries.GetAnwesenheitsListe;
using TvJahnOrchesterApp.Contracts.Termine.AnwesenheitsListe;

namespace TvJahnOrchesterApp.Api.Controllers.TerminControllers
{
    public class TerminAnwesenheitsController: BaseTerminController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public TerminAnwesenheitsController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpGet("anwesenheit/{terminId}")]
        public async Task<IActionResult> GetForTermin(Guid terminId, CancellationToken cancellationToken)
        {
            var getAllTerminQuery = new GetAnwesenheitsListeQuery(terminId);
            var queryResponse = await sender.Send(getAllTerminQuery, cancellationToken);
            var response = mapper.Map<GetTerminAnwesenheitsListeResponse>(queryResponse);

            return Ok(response);
        }

        [HttpPut("anwesenheit/{terminId}")]
        public async Task<IActionResult> UpdateAnwesenheitTermin(Guid terminId, [FromBody] UpdateTerminAnwesenheitsListenRequest request, CancellationToken cancellationToken)
        {

        }
    }
}
