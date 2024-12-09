using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models.Asset
{
    public class AssetDeleteRequest
    {
        [Required]
        public string? PublicId { get; set; }
    }
}
