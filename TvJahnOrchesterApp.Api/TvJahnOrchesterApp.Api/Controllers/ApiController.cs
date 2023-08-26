using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class ApiController: ControllerBase
    {
    }
}
