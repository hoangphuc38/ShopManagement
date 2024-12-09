using Microsoft.AspNetCore.Http;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IImageService
    {
        Task<BaseResponse> UploadAsync(AssetRequest request);

        Task<BaseResponse> DeleteAsync(AssetDeleteRequest request);
    }
}
