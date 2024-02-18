using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Application.Features.Authorization.Models
{
    public record AuthenticationResult(
        string Id, 
        string Name,
        string Email,
        string Token,
        string RefreshToken,
        string[] UserRoles,
        string? Image
        );
}
