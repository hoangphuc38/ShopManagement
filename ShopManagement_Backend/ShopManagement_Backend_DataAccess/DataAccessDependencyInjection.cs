using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopManagement_Backend_DataAccess.DapperRepositories;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;
using ShopManagement_Backend_DataAccess.Persistance;
using ShopManagement_Backend_DataAccess.Repositories;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_DataAccess
{
    public static class DataAccessDependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);

            services.AddDapperConnection();

            services.AddRepositories();

            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IShopRepository, ShopRepository>();
            services.AddScoped<IShopDetailRepository, ShopDetailRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Dapper
            services.AddScoped<IProductDapRepository, ProductDapRepository>();
            services.AddScoped<IShopDetailDapRepository, ShopDetailDapRepository>();
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig = configuration.GetConnectionString("ShopManagement"); ;

            services.AddDbContext<ShopManagementDbContext>(options =>
            {
                options.UseSqlServer(databaseConfig);
            });
        }

        private static void AddDapperConnection(this IServiceCollection services)
        {
            services.AddSingleton<ShopManagementDapperContext>();
        }
    }
}
