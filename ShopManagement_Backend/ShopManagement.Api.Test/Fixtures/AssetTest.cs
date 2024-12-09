using ShopManagement_Backend_Application.Models.Asset;
using ShopManagement_Backend_Application.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Api.Test.Fixtures
{
    public class AssetTest
    {
        public AssetResponse assetRes = new AssetResponse
        {
            UrlImage = "Test",
            PublicId = "Test",
        };

        public AssetRequest assetRequest = new AssetRequest
        {
            File = null,
            Key = "Test",
        };

        public AssetDeleteRequest deleteRequest = new AssetDeleteRequest
        {
            PublicId = "Test",
        };
    }
}
