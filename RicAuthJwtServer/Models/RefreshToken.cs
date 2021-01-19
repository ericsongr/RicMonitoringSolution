using System;

namespace RicAuthJwtServer.Models
{
    public class RefreshToken
    {
        public long RefreshTokenId { get; set; }
        public string UserId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
    }
}
