﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.Token
{
    public class RefreshTokenResponse
    {
        public string? NewAccessToken { get; set; }
    }
}