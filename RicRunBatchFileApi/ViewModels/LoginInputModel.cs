// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.



namespace RicRunBatchFileApi.ViewModels
{
    public class LoginInputModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
        
        public string DeviceId { get; set; }

        public string Platform { get; set; }

    }
}