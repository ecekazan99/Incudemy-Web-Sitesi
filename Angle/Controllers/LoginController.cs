using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Angle.Models;
using Angle.Helpers;
using Microsoft.Data.SqlClient;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Web.Helpers;
using System.Net.Mail;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Data;

namespace Angle.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        public LoginController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginModel()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult LoginModel(LoginModel model)
        {

            var prms = new Dictionary<string, object>();
            var logResult = 0;
            prms.Add("UserEmail", model.Email);
            prms.Add("UserPassword", model.Password);
            if (model.Email != null && model.Password != null)
            {
                logResult = DataHelper.GetFromStoredProcedure<int>("Sp_Login_User", prms);

            }
            string ipAddress = HttpContext.Connection.LocalIpAddress.ToString();
            var logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            prms.Add("LastLoginDate", logDate);
            prms.Add("LastIpAddress", ipAddress);
            if (logResult == 1)
            {
                HttpContext.Session.SetString("UserEmail", model.Email);

                var forID2 = new Dictionary<string, object>();
                forID2.Add("UserEmail", HttpContext.Session.GetString("UserEmail"));
                var userid = DataHelper.GetFromStoredProcedure<int>("Sp_SelectIdByMail_User", forID2);

                HttpContext.Session.SetInt32("UserId", userid);
                var forName = new Dictionary<string, object>();
                forName.Add("UserId", userid);
                var user = DataHelper.ListFromStoredProcedure("Sp_DisplayUserById_User", forName);
                foreach (DataRow item in user.Rows)
                {
                    HttpContext.Session.SetString("UserName", item.Field<string>("FirstName")+" "+ item.Field<string>("LastName"));
                    HttpContext.Session.SetString("FileWay", item.Field<string>("FileWay"));
                }
                

                var logresult = DataHelper.RunFromStoredProcedure("Sp_Insert_UserLogin", prms);
                
                TempData["msg"] = "Login Successfully";
                return Redirect("/AfterLogin/Home");
            }
            else
            {
                TempData["msg"] = "Email or password does not match!";
                return View(model);
            }

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["msg"] = "Logout Successfully ";
            return Redirect("/Home/Index");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(LoginModel model)
        {


            var prms = new Dictionary<string, object>();
            var email = 1;
            prms.Add("UserEmail", model.Email);

            email = DataHelper.GetFromStoredProcedure<int>("Sp_Forgot_Password", prms);

            if (email == 0)
            {
                Guid guid =Guid.NewGuid();
                var newpassword = guid.ToString().Substring(0, 8);
                
                var forName = new Dictionary<string, object>();
                forName.Add("UserPassword", newpassword);
                forName.Add("UserEmail", model.Email);
               
                var save = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Forgot_Password", forName);

               
                System.Net.Mail.MailMessage Msg = new System.Net.Mail.MailMessage();

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress("notify@monovi.com.tr");
                msg.To.Add("onur.mnov@gmail.com"); //Şifrenin gideceği mail adresi
                msg.IsBodyHtml = true;
                msg.Subject = "Password Change Request";
                msg.Body += "Hello "+ "<br/> Your Password:" + newpassword;
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);

                client.UseDefaultCredentials = true;
                client.EnableSsl = true;
                NetworkCredential net = new NetworkCredential("notify@monovi.com.tr", "Password35Kalkan!");
                client.Credentials = net;


                client.Send(msg);
                
                ViewBag.Message = "Your Password Has Been Sent Successfully";
                return Redirect("/Login/LoginModel");
            }
            else
            {
                ViewBag.Message = "User doesn't exists";
            }
            return View();
        }

    }
}
