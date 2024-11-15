using System;
using API.Entities;

namespace API.Sevices;

public class TokenService(IConfiguration config) : ITokenService 
{

    public string CreateToken(AppUser user)
    {
        // Implement logic to generate JWT token
        // For demonstration purposes, let's use a simple string for the token
        var tokenKey = config["TokenKeyt"] ?? throw new Exception("No token key from app settings");
        if (tokenKey.Length < 64) throw new Exception("Your key must be at least 64 characters");
        var key
    }

}

public interface ITokenService
{
}