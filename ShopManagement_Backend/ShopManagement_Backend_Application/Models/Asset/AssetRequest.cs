using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.Asset
{
    public class AssetRequest
    {
        public string Key { get; set; }

        public IFormFile? File { get; set; }
    }
}
