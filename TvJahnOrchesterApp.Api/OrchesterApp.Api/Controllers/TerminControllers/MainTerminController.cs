using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Termin.Commands.Create;
using TvJahnOrchesterApp.Application.Termin.Commands.Delete;
using TvJahnOrchesterApp.Application.Termin.Commands.Update;
using TvJahnOrchesterApp.Application.Termin.Queries.GetAll;
using TvJahnOrchesterApp.Application.Termin.Queries.GetSpecific;
using TvJahnOrchesterApp.Contracts.Termine.Main;

namespace TvJahnOrchesterApp.Api.Controllers.TerminControllers
{
    public class MainTerminController : BaseTerminController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public MainTerminController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var getAllTerminQuery = new GetAllTermineQuery();
            var queryResponse = await sender.Send(getAllTerminQuery, cancellationToken);
            var response = mapper.Map<GetAllTerminResponse[]>(queryResponse);

            return Ok(response);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetTermin(Guid id, CancellationToken cancellationToken)
        {
            var getTerminQuery = new GetTerminByIdQuery(id);
            var queryResponse = await sender.Send(getTerminQuery);
            var response = mapper.Map<GetTerminResponse>(queryResponse);

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTermin(CreateTerminRequest createTerminRequest, CancellationToken cancellationToken)
        {
            var createCommand = mapper.Map<CreateTerminCommand>(createTerminRequest);
            var terminResult = await sender.Send(createCommand);
            var response = mapper.Map<CreateTerminResponse>(terminResult);

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTermin(UpdateTerminRequest updateTerminRequest, CancellationToken cancellationToken)
        {
            var updateCommand = mapper.Map<UpdateTerminCommand>(updateTerminRequest);
            var updateResult = await sender.Send(updateCommand);
            var response = mapper.Map<UpdateTerminResponse>(updateResult);

            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTermin(Guid id, CancellationToken cancellationToken)
        {
            var deleteCommand = new DeleteTerminCommand(id);
            var deleteResult = await sender.Send(deleteCommand);

            return Ok(deleteResult);
        }
    }
}
