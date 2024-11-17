using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            // Ensure the configuration key is correctly retrieved
            var tokenKey = config["TokenKey"] ?? throw new Exception("No token key in app settings");
            
            // Enforce a minimum security length for the token key
            if (tokenKey.Length < 64)
                throw new Exception("Your token key must be at least 64 characters long");
            
            // Initialize the symmetric security key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public string CreateToken(AppUser user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserName))
                throw new ArgumentNullException(nameof(user), "User or username cannot be null or empty");

            // Define claims for the JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            // Create signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Set up the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration
                SigningCredentials = creds
            };

            // Create and write the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
