using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Commands.UpdateAnwesenheit;
using TvJahnOrchesterApp.Application.Termin.Common;
using TvJahnOrchesterApp.Application.Termin.Queries.GetAllAnwesenheitsListe;
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

        [HttpGet("anwesenheit/all")]
        public async Task<IActionResult> GetForAllTermin(CancellationToken cancellationToken)
        {
            var getAllTerminQuery = new GetAllAnwesenheitsListeQuery();
            var queryResponse = await sender.Send(getAllTerminQuery, cancellationToken);
            var response = mapper.Map<GetAnwesenheitsListeResponse>(queryResponse);

            return Ok(response);
        }

        [HttpPut("anwesenheit/{terminId}")]
        public async Task<IActionResult> UpdateAnwesenheitTermin(Guid terminId, [FromBody] UpdateTerminAnwesenheitsListenRequest request, CancellationToken cancellationToken)
        {
            var updateAnwesenheitsListe = mapper.Map<UpdateAnwesenheitsEintrag[]>(request.TerminAnwesenheitsListe);
            var updateAnwesenheitsCommand = new UpdateAnwesenheitCommand(terminId, updateAnwesenheitsListe);
            var updateResult = await sender.Send(updateAnwesenheitsCommand, cancellationToken);
            var response = mapper.Map<UpdateTerminAnwesenheitsListenResponse>(updateResult);

            return Ok(response);

        }
    }
}
