using System.Collections.Generic;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        void Delete(string userId, string deviceId);
        void Delete(string userId);
        Dictionary<string, int> GenerateRefreshToken(string userId, string deviceId);
        RefreshToken IsRefreshTokenValid(string token);
        void Save(RefreshToken item);
    }
}
