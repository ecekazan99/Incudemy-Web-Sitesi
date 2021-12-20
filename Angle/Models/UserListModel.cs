using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class UserListModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string UserRoleId { get; set; }
        public string UniversityId{ get; set; }
        public string UniversityName{ get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string IncudemistLevelId { get; set; }
    }
}
