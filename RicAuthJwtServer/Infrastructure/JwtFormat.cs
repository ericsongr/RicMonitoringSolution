using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.WebUtilities;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace RicAuthJwtServer.Infrastructure
{
    public class JwtFormat
    {
        private readonly string _issuer = string.Empty;

        public JwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string GenerateAccessToken(string audienceId, string audienceSecret, string email, int timeOutInMinutes, string number, string name, string role)
        {
            if (string.IsNullOrEmpty(audienceId)) throw new ArgumentNullException(nameof(audienceId));
            if (string.IsNullOrEmpty(audienceSecret)) throw new ArgumentNullException(nameof(audienceSecret));

            var symmetricKeyAsBase64 = audienceSecret;
            var keyByteArray = WebEncoders.Base64UrlDecode(symmetricKeyAsBase64);
            var signingKey = new SigningCredentials(new SymmetricSecurityKey(keyByteArray), SecurityAlgorithms.HmacSha256Signature);

            var now = DateTime.UtcNow;
            var issued = now;

            var expires = (int)(now.AddMinutes(timeOutInMinutes) - now).TotalSeconds;

            var claims = new Claim[]
            {    
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("number", number),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, expires.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(_issuer, audienceId, claims, issued, issued.AddMinutes(timeOutInMinutes), signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }
    }
}
