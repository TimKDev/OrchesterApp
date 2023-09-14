using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using TvJahnOrchesterApp.Application.Common.Interfaces.Authentication;
using TvJahnOrchesterApp.Domain.UserAggregate;
using TvJahnOrchesterApp.Infrastructure.Common.Interfaces;

namespace TvJahnOrchesterApp.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IJwtHandler jwtHandler,
            UserManager<User> userManager
        )
        {
            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task AddRefreshTokenToUserInDBAsync(User user, string refreshToken)
        {
            user.SetRefreshToken(refreshToken);
            await _userManager.UpdateAsync(user);
        }

        public bool IsRefreshTokenValid(User user, string refreshToken)
        {
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public async Task RevokeRefreshTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.SetRefreshToken(null);
            await _userManager.UpdateAsync(user);
        }
    }
}
