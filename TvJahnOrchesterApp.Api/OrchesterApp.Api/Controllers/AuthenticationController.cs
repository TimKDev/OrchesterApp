using BuberDinner.Api.Controllers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvJahnOrchesterApp.Application.Authentication.Commands.Register;
using TvJahnOrchesterApp.Application.Authentication.Queries.Login;
using TvJahnOrchesterApp.Contracts.Authentication;

namespace TvJahnOrchesterApp.Api.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public AuthenticationController(ISender sender, IMapper mapper)
        {
            this.mapper = mapper;
            this.sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var registerCommand = mapper.Map<RegisterCommand>(registerRequest);
            var result = await sender.Send(registerCommand);

            var registerResponse = mapper.Map<RegisterResponse>(registerCommand);

            return Ok(registerResponse);
              
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = mapper.Map<LoginQuery>(request);
            var authResult = await sender.Send(query);

            var loginResponse = mapper.Map<LoginResponse>(authResult);
            return Ok(loginResponse);
        }
    }
}
