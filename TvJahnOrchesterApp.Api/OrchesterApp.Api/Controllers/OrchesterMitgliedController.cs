using BuberDinner.Api.Controllers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Api.Controllers
{

    [AllowAnonymous]
    public class OrchesterMitgliedController : ApiController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public OrchesterMitgliedController(ISender sender, IMapper mapper)
        {
            this.sender = sender;
            this.mapper = mapper;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync (CancellationToken cancellationToken)
        //{
        //    var allOrchesterMitglieder = await sender.Send(new GetAllOrchesterMitgliederQuery());
        //    return Ok(allOrchesterMitglieder);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateOrchesterMitgliedRequest createOrchesterMitgliedRequest, CancellationToken cancellationToken)
        {
            var createOrchesterMitgliedCommand = mapper.Map<CreateOrchesterMitgliedCommand>(createOrchesterMitgliedRequest);

            var orchesterMitglied = await sender.Send(createOrchesterMitgliedCommand);
            var result = mapper.Map<CreateOrchesterMitgliedResponse>(orchesterMitglied);
            return Ok(result);
        }
    }
}
