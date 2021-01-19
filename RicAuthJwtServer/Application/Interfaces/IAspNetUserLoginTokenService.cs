using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Application.Interfaces
{
    public interface IAspNetUserLoginTokenService
    {
        void Delete(string userId);
        AspNetUserLoginToken GetLoginToken(string userId);
        string GenerateLoginToken(string userId);
    }
}
