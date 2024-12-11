using ShopManagement_Backend_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Api.Test.Fixtures
{
    public class RoleTest
    {
        public Role role = new Role
        {
            RoleId = 1,
            RoleName = "Test",
        };

        public Role roleNull = null;
    }
}
