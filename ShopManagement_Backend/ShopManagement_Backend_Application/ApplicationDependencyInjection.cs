using Microsoft.Extensions.DependencyInjection;
using ShopManagement_Backend_Application.DapperServices;
using ShopManagement_Backend_Application.DapperServices.Interfaces;
using ShopManagement_Backend_Application.MappingProfiles;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement_Backend_Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();

            services.AddAutoMapper();

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IShopDetailService, ShopDetailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            //Dapper
            services.AddScoped<IProductDapService, ProductDapService>();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMappingProfileMarker));
        }
    }
}
