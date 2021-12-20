using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Angle.Models;
using Angle.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Angle.Controllers
{
    public class GiveFeedbackController : Controller
    {

        private readonly IHostingEnvironment _appEnvironment;


        public GiveFeedbackController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }


        public IActionResult GiveFeedback()
        {
            GiveFeedbackModel model = new GiveFeedbackModel();
            var givefeedback = new Dictionary<string, object>();
            var feedbackResult = DataHelper.ListFromStoredProcedure("Sp_Display_FeedbackType", givefeedback);

            foreach (DataRow item in feedbackResult.Rows)
            {
                FeedbackType u = new FeedbackType();
                u.FeedbackTypeId = item.Field<int>("FeedbackTypeId");
                u.FeedbackTypeName = item.Field<string>("FeedbackTypeName");
                model.FeedbackTypes.Add(u);
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult GiveFeedback(Angle.Models.GiveFeedbackModel model)
        {

            string body = string.Empty;
            var root = _appEnvironment.WebRootPath;
            using (var reader = new StreamReader(root + @"/emailtemplate/template.html"))
            {
                body = reader.ReadToEnd();

            }


            var prms = new Dictionary<string, object>();
            prms.Add("FeedbackType", model.FeedbackTypes);
            prms.Add("Description", model.Description);
            var var1 = DataHelper.ListFromStoredProcedure("Sp_Insert_Feedback", prms);
            var var2 = DataHelper.ListFromStoredProcedure("Sp_Insert_FeedbackType", prms);

            try
            {

                TempData["Message"] = "Geri bildiriminiz gönderilmiştir.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Hata: " + ex.Message;
            }

            return Redirect("/ViewFeedback/ViewFeedback");
        }




    }
}
