using System;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Models;

namespace RicAuthJwtServer.Application
{
    public class AspNetUserLoginTokenService : IAspNetUserLoginTokenService
    {
        private readonly IAspNetUserLoginTokenRepository _loginTokenRepository;

        public AspNetUserLoginTokenService(IAspNetUserLoginTokenRepository loginTokenRepository)
        {
            _loginTokenRepository = loginTokenRepository ?? throw new ArgumentNullException(nameof(loginTokenRepository));
        }

        public void Delete(string userId)
        {
            _loginTokenRepository.DeleteWhere(o => o.UserId == userId);
            _loginTokenRepository.Commit();
        }

        public AspNetUserLoginToken GetLoginToken(string userId)
        {
            var result = _loginTokenRepository.GetSingleAsync(o => o.UserId == userId).Result;
            if (result == null)
                return null;

            return result;
        }

        public string GenerateLoginToken(string userId)
        {
            // Delete any user login token
            Delete(userId);

            string xoken = $"{userId.Replace("-", "")}{DateTime.Now.ToString("ddMMyyyyhhmmss")}";
            var token = new AspNetUserLoginToken
            {
                UserId = userId,
                CreatedDateUtc = DateTime.UtcNow,
                TokenValue = xoken
            };
            _loginTokenRepository.Add(token);
            _loginTokenRepository.Commit();
            return xoken;
        }
        
    }
}
