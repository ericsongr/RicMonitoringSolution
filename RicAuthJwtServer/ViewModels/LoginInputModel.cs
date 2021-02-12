// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace RicAuthJwtServer.ViewModels
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        
        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string Platform { get; set; }

        //public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}