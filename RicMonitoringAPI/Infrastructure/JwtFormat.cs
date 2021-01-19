using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Claims;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using SecurityAlgorithms = System.IdentityModel.Tokens.SecurityAlgorithms;
using SymmetricSecurityKey = System.IdentityModel.Tokens.SymmetricSecurityKey;

namespace RicMonitoringAPI.Infrastructure
{
    public class JwtFormat
    {
        private readonly string _issuer = string.Empty;

        public JwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string GenerateToken(string audienceId, string audienceSecret, string email, int timeOutInMinutes, string number, string memberName)
        {
            if (string.IsNullOrEmpty(audienceId)) throw new ArgumentNullException(nameof(audienceId));
            if (string.IsNullOrEmpty(audienceSecret)) throw new ArgumentNullException(nameof(audienceSecret));

            var symmetricKeyAsBase64 = audienceSecret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var signingKey = new SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyByteArray), SecurityAlgorithms.HmacSha256Signature);

            var now = DateTime.UtcNow;
            var issued = now;
            var expires = (int)(now.AddMinutes(timeOutInMinutes) - now).TotalSeconds;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("number", number),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, expires.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimsIdentity.DefaultNameClaimType, memberName),
            };

            var token = new JwtSecurityToken(_issuer, audienceId, claims, issued, issued.AddMinutes(timeOutInMinutes), signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }
    }
}
