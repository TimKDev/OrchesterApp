using BuberDinner.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TvJahnOrchesterApp.Api.Controllers
{
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            return Ok("Register successful");
        }
    }
}
