using System;
using System.Collections.Generic;
using System.Data;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data.Exception;
using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Infrastructure;
using RicAuthJwtServer.Models;
using RicCommon.Diagnostics;

namespace RicAuthJwtServer.Application
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        }

        public void Delete(string userId, string deviceId)
        {
            _refreshTokenRepository.DeleteWhere(o => o.UserId == userId && o.DeviceId == deviceId);
            _refreshTokenRepository.Commit();
        }

        public void Delete(string userId)
        {
            _refreshTokenRepository.DeleteWhere(o => o.UserId == userId);
            _refreshTokenRepository.Commit();
        }

        public Dictionary<string, int> GenerateRefreshToken(string userId, string deviceId)
        {
            // Delete any user refresh token
            Delete(userId, deviceId);

            Dictionary<string, int> result = new Dictionary<string, int>();
            DateTime now = DateTime.UtcNow;
            int expires_in_seconds = 500 * 60; //_settingService.GetAsInt(SettingName.RefreshTokenExpiryMinutes) * 60;
            string xoken = Encryption.GetHash(Guid.NewGuid().ToString());
            var token = new RefreshToken
            {
                IssuedUtc = now,
                ExpiresUtc = now.AddSeconds(expires_in_seconds),
                Token = xoken,
                UserId = userId,
                DeviceId = deviceId
            };
            _refreshTokenRepository.Add(token);
            _refreshTokenRepository.Commit();
            result.Add(Encryption.Encrypt(token.Token), expires_in_seconds);
            return result;
        }

        public RefreshToken IsRefreshTokenValid(string token)
        {
            try
            {
                string xoken = Encryption.Decrypt(token);
                var validateToken = _refreshTokenRepository.GetSingleAsync(o => o.Token == xoken).GetAwaiter().GetResult();
                if (validateToken == null)
                    return null;

                DateTime now = DateTime.UtcNow;
                if (now > validateToken.ExpiresUtc)
                    return null;

                return validateToken;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex);
                return null;
            }
        }

        public void Save(RefreshToken item)
        {
            try
            {
                var token = new RefreshToken
                {
                    ExpiresUtc = item.ExpiresUtc,
                    IssuedUtc = item.IssuedUtc,
                    Token = item.Token,
                    UserId = item.UserId,
                    DeviceId = item.DeviceId
                };
                _refreshTokenRepository.Add(token);
                _refreshTokenRepository.Commit();

            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorInCreate();
            }
        }

    }
}
