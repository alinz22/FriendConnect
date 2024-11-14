using System;

namespace API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppContext user);
}
