using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_DataAccess.Persistance
{
    public class ShopManagementDapperContext 
    {
        private readonly IConfiguration _config;

        public ShopManagementDapperContext(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("ShopManagement"));
        }
    }
}
