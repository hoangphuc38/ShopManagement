using Microsoft.Extensions.DependencyInjection;
using ShopManagement_Backend_Application.Helpers;
using ShopManagement_Backend_Application.MappingProfiles;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();

            services.AddAutoMapper();

            services.AddSignalR();

            return services;
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShopService, ShopService>();
            services.AddScoped<IShopDetailService, ShopDetailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IJwtHelper, JwtHelper>();

            services.AddScoped<PaginationHelper<Product>>();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(IMappingProfileMarker));
        }
    }
}
