using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using dotenv.net;
using Microsoft.Extensions.Configuration;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Application.Models.Asset;

namespace ShopManagement_Backend_Application.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IConfiguration config)
        {
            //DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            string url = config["CloudinarySettings:CloudinaryUrl"];
            _cloudinary = new Cloudinary(url);
            _cloudinary.Api.Secure = true;
        }

        public async Task<BaseResponse> UploadAsync(AssetRequest request)
        {
            var uploadResult = new ImageUploadResult();

            if (request.File == null)
            {
                return new BaseResponse(StatusCodes.Status400BadRequest, "File not found");
            }

            if (request.File.Length <= 0)
            {
                return new BaseResponse(StatusCodes.Status400BadRequest, "File does not have content");
            }

            if (request.File.Length > ImageConstant.LimitSize)
            {
                return new BaseResponse(StatusCodes.Status400BadRequest, "Attachment limit size reached");
            }

            using (var stream = request.File.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(request.File.FileName, stream),
                    Transformation = new Transformation()
                                        .Height(400)
                                        .Width(400)
                                        .Crop("fill")
                                        .Gravity("face"),
                    Folder = request.Key
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return new BaseResponse
            {
                Status = 200,
                Message = "Success",
                Data = uploadResult.Url.ToString()
            };
        }
    }
}
