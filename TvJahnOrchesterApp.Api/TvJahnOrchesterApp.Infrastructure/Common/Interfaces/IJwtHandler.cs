using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TvJahnOrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Infrastructure.Common.Interfaces
{
    public interface IJwtHandler
    {
        SigningCredentials GetSigningCredentials();
        Task<List<Claim>> GetClaims(User user);
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}