using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class ViewFeedbackModel
    {
        public ViewFeedbackModel()
        {
            this.RoleTypes = new List<Role>();
        }
        public List<Role> RoleTypes { get; set; }
    }
   
}
