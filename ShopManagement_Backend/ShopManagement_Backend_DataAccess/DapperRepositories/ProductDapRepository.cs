using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.DapperRepositories.Interfaces;
using ShopManagement_Backend_DataAccess.Persistance;
using static Dapper.SqlMapper;

namespace ShopManagement_Backend_DataAccess.DapperRepositories
{
    public class ProductDapRepository : IProductDapRepository
    {
        protected readonly ShopManagementDapperContext Context;

        public ProductDapRepository(ShopManagementDapperContext context)
        {
            Context = context;
        }

        public int AddAsync(Product entity)
        {
            try
            {
                using var connection = Context.CreateConnection();

                return connection.Execute(
                "INSERT INTO PRODUCT (ProductName, Price, IsDeleted) VALUES (@ProductName, @Price, @IsDeleted)",
                entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int DeleteAsync(Product entity)
        {
            try
            {
                using var connection = Context.CreateConnection();

                return connection.Execute(
                    $"UPDATE PRODUCT SET IsDeleted = 1 WHERE ProductId = @ProductId; " +
                    "UPDATE SHOPDETAIL SET IsDeleted = 1 WHERE ProductId = @ProductId;",
                    entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public IEnumerable<Product> GetAllAsync()
        {
            try
            {
                using var connection = Context.CreateConnection();

                return connection.Query<Product>(
                    "SELECT ProductID, ProductName, Price FROM PRODUCT WHERE IsDeleted = 0");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Product? GetFirstOrNullAsync(object id)
        {
            try
            {
                using var connection = Context.CreateConnection();

                return connection.QueryFirstOrDefault<Product>(
                    "SELECT ProductID, ProductName, Price FROM PRODUCT WHERE ProductID = @Id AND IsDeleted = 0",
                    new { id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateAsync(Product entity)
        {
            try
            {
                using var connection = Context.CreateConnection();

                return connection.Execute(
                    $"UPDATE PRODUCT SET ProductName = @ProductName, Price = @Price" +
                    $"WHERE ProductId = @ProductId",
                    entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
