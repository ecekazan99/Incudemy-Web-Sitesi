using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Angle.Models;
using Angle.Helpers;

namespace Angle.Controllers
{
    public class RegisteredEventController : Controller
    {
        public IActionResult RegisteredEvent()
        {
            var prms = new Dictionary<string, object>();
           // var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_Event", prms);
            return View();
        }
    }
}
