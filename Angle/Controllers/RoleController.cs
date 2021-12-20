using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Angle.Models;
using Angle.Helpers;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Angle.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleModel()
        {
            RoleModel model = new RoleModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult RoleModel(RoleModel model)
        {
            return View(model);
        }
    }
}
