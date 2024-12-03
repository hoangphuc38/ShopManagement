using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.Models
{
    public class RepoResponse
    {
        public object? Result { get; set; }

        public int TotalRecords { get; set; }
    }
}
