using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OrchesterApp.Domain.UserAggregate;

namespace TvJahnOrchesterApp.Application.Common.Interfaces.Services
{
    public interface IJwtHandler
    {
        SigningCredentials GetSigningCredentials();
        Task<List<Claim>> GetClaims(User user);
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}