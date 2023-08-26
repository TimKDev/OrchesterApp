using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;
using TvJahnOrchesterApp.Application.Services;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Api.Controllers
{
    public class OrchestraMemberController: ControllerBase
    {
        private readonly ISender sender;

        public OrchestraMemberController(ISender sender)
        {
            this.sender = sender;
        }

        public async Task<IActionResult> CreateAsync(OrchestraMemberContract orchesterMemberContract, CancellationToken none)
        {
            if (!orchesterMemberContract.IsValid())
            {
                return BadRequest("Invalid Contract");
            }
            var createOrchestraMemberCommand = new CreateOrchestraMemberCommand
            {
                FirstName = orchesterMemberContract.FirstName,
                LastName = orchesterMemberContract.LastName,
                InstrumentIds = orchesterMemberContract.InstrumentIds,
                PhoneNumber = orchesterMemberContract.PhoneNumber,
                Street = orchesterMemberContract.Address.Street,
                HouseNumber = orchesterMemberContract.Address.HouseNumber,
                City = orchesterMemberContract.Address.City,
                Zip = orchesterMemberContract.Address.Zip,
            };

            await sender.Send(createOrchestraMemberCommand);
            return Ok("");
        }
    }
}
