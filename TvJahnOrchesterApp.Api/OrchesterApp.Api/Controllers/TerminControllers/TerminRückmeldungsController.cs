using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Commands.Rückmeldung;
using TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungForOtherUser;
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

        [HttpPut("rückmeldung")]
        public async Task<IActionResult> RückmeldungTermin(RückmeldungTerminRequest rückmeldungTerminRequest, CancellationToken cancellationToken)
        {
            var rückmeldungsCommand = mapper.Map<RückmeldungCommand>(rückmeldungTerminRequest);
            var commandResponse = await sender.Send(rückmeldungsCommand, cancellationToken);
            var response = mapper.Map<RückmeldungTerminResponse>(commandResponse);

            return Ok(response);
        }

        [HttpPut("rückmeldung/forUser")]
        public async Task<IActionResult> RückmeldungTermin(RückmeldungTerminForOtherUserRequest rückmeldungTerminForOtherUserRequest, CancellationToken cancellationToken)
        {
            var rückmeldungsCommand = mapper.Map<RückmeldungForOtherUserCommand>(rückmeldungTerminForOtherUserRequest);
            var commandResponse = await sender.Send(rückmeldungsCommand, cancellationToken);
            var response = mapper.Map<RückmeldungTerminResponse>(commandResponse);

            return Ok(response);
        }
    }
}
