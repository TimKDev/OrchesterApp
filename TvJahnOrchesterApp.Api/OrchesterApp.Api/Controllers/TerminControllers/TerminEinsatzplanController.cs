using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanUpdate;
using TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockDelete;
using TvJahnOrchesterApp.Application.Termin.Commands.EinsatzplanZeitblockUpdate;
using TvJahnOrchesterApp.Contracts.Termine.Einsatzplan;

namespace TvJahnOrchesterApp.Api.Controllers.TerminControllers
{
    public class TerminEinsatzplanController : BaseTerminController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public TerminEinsatzplanController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpPut("EinsatzPlan/{terminId}")]
        public async Task<IActionResult> UpdateTerminEinsatzplanMainInfo(Guid terminId, [FromBody] UpdateEinsatzplanRequest request, CancellationToken cancellationToken)
        {
            var updateCommand = mapper.Map<EinsatzplanUpdateCommand>((request, terminId));
            await sender.Send(updateCommand);
            var response = mapper.Map<UpdateEinsatzplanResponse>(request);

            return Ok(response);

        }

        [HttpPost("EinsatzPlan/{terminId}/Zeitblock")]
        public async Task<IActionResult> UpdateCreateTerminEinsatzplanZeitblock(Guid terminId, [FromBody] UpdateCreateZeitblockRequest request, CancellationToken cancellationToken)
        {
            var updateZeitblockCommand = mapper.Map<EinsatzplanZeitblockUpdateCommand>((request, terminId));
            await sender.Send(updateZeitblockCommand);
            var response = mapper.Map<UpdateCreateZeitblockResponse>(request);

            return Ok(response);
        }

        [HttpDelete("EinsatzPlan/{terminId}/Zeitblock/{zeitBlockId}")]
        public async Task<IActionResult> DeleteEinsatzplanZeitblock(Guid terminId, Guid zeitBlockId, CancellationToken cancellationToken)
        {
            var deleteZeitBlockCommand = new EinsatzplanZeitblockDeleteCommand(terminId, zeitBlockId);
            var result = await sender.Send(deleteZeitBlockCommand);

            return Ok(result);

        }


    }
}
