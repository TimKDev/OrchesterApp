using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser;
using TvJahnOrchesterApp.Application.Termin.Queries.GetRückmeldungenTermin;
using TvJahnOrchesterApp.Contracts.Termine.Rückmeldung;

namespace TvJahnOrchesterApp.Api.Controllers.TerminControllers
{
    public class TerminRückmeldungsController: BaseTerminController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public TerminRückmeldungsController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpGet("Rückmeldung/{terminId}")]
        public async Task<IActionResult> GetRückmeldungenTermin(Guid terminId, CancellationToken cancellationToken)
        {
            var getRückmeldungsQuery = new GetRückmeldungenTerminQuery(terminId);
            var result = await sender.Send(getRückmeldungsQuery);
            var response = mapper.Map<GetRückmeldungenTerminResponse>(result);

            return Ok(response);
        }

        [HttpPut("Rückmeldung")]
        public async Task<IActionResult> RückmeldungTermin(RückmeldungTerminRequest rückmeldungTerminRequest, CancellationToken cancellationToken)
        {
            var rückmeldungsCommand = mapper.Map<RückmeldungCommand>(rückmeldungTerminRequest);
            var commandResponse = await sender.Send(rückmeldungsCommand, cancellationToken);
            var response = mapper.Map<RückmeldungTerminResponse>(commandResponse);

            return Ok(response);
        }

        [HttpPut("Rückmeldung/forUser")]
        public async Task<IActionResult> RückmeldungTermin(RückmeldungTerminForOtherUserRequest rückmeldungTerminForOtherUserRequest, CancellationToken cancellationToken)
        {
            var rückmeldungsCommand = mapper.Map<RückmeldungForOtherUserCommand>(rückmeldungTerminForOtherUserRequest);
            var commandResponse = await sender.Send(rückmeldungsCommand, cancellationToken);
            var response = mapper.Map<RückmeldungTerminResponse>(commandResponse);

            return Ok(response);
        }

        [HttpPut("Rückmeldung/{terminId}/ChangeInstrumentsAndNotes")]
        public async Task<IActionResult> ChangeInstrumentsAndNotes(Guid terminId, [FromBody] RückmeldungenChangeRequest request, CancellationToken cancellationToken)
        {
            var changeRückmeldungCommand = mapper.Map<RückmeldungChangeInstrumentsAndNotesCommand>((terminId, request));
            await sender.Send(changeRückmeldungCommand);

            return Ok(request);
        }
    }
}
