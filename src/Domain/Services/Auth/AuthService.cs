using Domain.Services.Security;
using Domain.Services.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Microsoft.Extensions.Options;

namespace Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsersService _usersService;
        private readonly IEncryptService _encryptService;
        private readonly Configs _configs;

        public AuthService(IUsersService usersService, IEncryptService encryptService,  IOptions<Configs> configs)
        {
            _usersService = usersService;
            _encryptService = encryptService;
            _configs = configs.Value;
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _usersService.GetUserWithHashByNameAsync(loginDto.UserName);
            if (user == null)
            {
                throw new Exception($"User '{ loginDto.UserName}' not found.");
            }

            var passwordHash = _encryptService.Encrypt(loginDto.Password, _configs.Encryptkey);
            if (passwordHash != user.PasswordHash)
            {
                throw new Exception("Invalid username or password");
            }

            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.UserName),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configs.JwtKey));
            var jwt = new JwtSecurityToken(
                issuer: _configs.JwtIssuer,
                audience: _configs.JwtAudience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(_configs.JwtDurationInMinutes)), new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new LoginResultDto
            {
                Token = encodedJwt
            };
        }
    }
}
