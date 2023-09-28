using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Contracts.Authentication
{
    public record LoginResponse(Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Token, 
        string RefreshToken);
}
