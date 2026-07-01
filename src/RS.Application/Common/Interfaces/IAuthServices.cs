using System;
using RS.Domain.Entities;

namespace RS.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }

    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
}
