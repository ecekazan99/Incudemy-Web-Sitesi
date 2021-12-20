using Angle.Helpers;
using Angle.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Angle.Controllers
{
    public class SendEmailController : Controller
    {



        private readonly IHostingEnvironment _appEnvironment;
        public SendEmailController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }



        public IActionResult SendEmail(EventType search, Category search2, Role search3)
        {





            SendEmail model = new SendEmail();



            var getEvent = new Dictionary<string, object>();
            var getRole = new Dictionary<string, object>();
            var filter = DataHelper.ListFromStoredProcedure("Sp_Display_Role", getRole);
            var filter1 = DataHelper.ListFromStoredProcedure("Sp_Display_EventTypes", getEvent);
            var filter2 = DataHelper.ListFromStoredProcedure("Sp_Display_Categories", getEvent);

            foreach (DataRow item in filter1.Rows)
            {
                EventType u = new EventType();



                u.EventTypesId = item.Field<int>("EventTypesId");



                u.EventTypesName = item.Field<string>("EventTypesName");



                model.EventTypes.Add(u);



            }
            foreach (DataRow item in filter2.Rows)
            {
                Category u = new Category();



                u.CategoryId = item.Field<int>("CategoryId");



                u.CategoryName = item.Field<string>("CategoryName");



                model.Categories.Add(u);



            }
            foreach (DataRow item in filter.Rows)
            {
                Role u = new Role();



                u.RoleId = item.Field<int>("RoleId");



                u.UserRoleName = item.Field<string>("UserRoleName");



                model.Roles.Add(u);






                if (search != null && search2 != null)
                {



                    model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) /*&& x.Role*/).ToList();



                }



                //model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.RoleType).ToList();




                //if (search != null && search2 != null)
                //{
                //model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.RoleType).ToList();


                //if (search != null && search2 != null)
                //{
                //    model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.Roles.Contains(search3).ToList());



                if (search != null && search2 != null)
                {

                    model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) /*&& x.Role*/).ToList();

                }

                //model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.RoleType).ToList();

                //if (search != null && search2 != null)
                //{
                //model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.RoleType).ToList();


                //if (search != null && search2 != null)
                //{
                //    model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2) && x.Roles.Contains(search3).ToList());


                //}



                //}








                return View(model);

            }
            //[HttpPost]
            //public ActionResult SendEmail(Angle.Models.SendEmail model)
            {


                string body = string.Empty;
                var root = _appEnvironment.WebRootPath;
                using (var reader = new StreamReader(root + @"/emailtemplate/template.html"))
                {
                    body = reader.ReadToEnd();




                }




                MailMessage email = new MailMessage();
                email.To.Add("notify@monovi.com.tr");     //mailin gideceği hesap
                email.From = new MailAddress("notify@monovi.com.tr");
                email.Subject = "Hello, Incudemist!";
                email.Body = model.mail + body;
                email.IsBodyHtml = true;



                //}

                //}



                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";       //Gmail hesapları icin geçerli
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("notify@monovi.com.tr", "SIFRE");    //hesabın şifresi girilmeli




                try
                {
                    smtp.Send(email);
                    TempData["Message"] = "Mailiniz iletilmiştir.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Mail gönderilemedi: " + ex.Message;
                }



                return View();
            }
        }
    }
}
