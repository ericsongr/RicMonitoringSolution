using System;
using System.ComponentModel.DataAnnotations;

namespace RicAuthJwtServer.Models
{
    public class AspNetUserLoginToken
    {
        [Key]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string TokenValue { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
