using Angle.Helpers;
using Angle.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Controllers
{
    public class ViewFeedbackController : Controller
    {
        public IActionResult ViewFeedback()
        {
            ViewFeedbackModel model = new ViewFeedbackModel();

            var viewFeedback = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_Role", viewFeedback);

            foreach (DataRow item in result1.Rows)
            {
                Role u = new Role();

                u.RoleId = item.Field<int>("RoleId");

                u.UserRoleName = item.Field<string>("UserRoleName");

                model.RoleTypes.Add(u);

            }
            return View(model);
        }
    }
}
