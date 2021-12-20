using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace Angle.Models
{
    public class Role
    {

        public Role(){

            this.Roles = new List<Role>();

        }


        public int RoleId { get; set; }
        public string UserRoleName { get; set; }

        public List<Role> Roles { get; set; }
    }
}
