using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Web;
using Angle.Models;
using Angle.Helpers;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using System.IO;

namespace Angle.Controllers
{
    public class EventController : Controller
    {


        public ActionResult Event()
        {

            EventModel model = new EventModel();

            var createEvent = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_EventTypes", createEvent);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_Display_Categories", createEvent);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_Display_EventLevel", createEvent);
            var forID2 = new Dictionary<string, object>();
            var forNameId = new Dictionary<string, object>();
            forID2.Add("UserEmail", HttpContext.Session.GetString("UserEmail"));

            var userid = DataHelper.GetFromStoredProcedure<int>("Sp_SelectIdByMail_User", forID2);
            HttpContext.Session.SetInt32("UserId", userid);
            forNameId.Add("UserID", userid);
            var userName = DataHelper.GetFromStoredProcedure<string>("Sp_DisplayUserById_User", forNameId);
            HttpContext.Session.SetString("UserName", userName);
            foreach (DataRow item in result1.Rows)
            {
                EventType u = new EventType();

                u.EventTypesId = item.Field<int>("EventTypesId");

                u.EventTypesName = item.Field<string>("EventTypesName");

                model.EventTypes.Add(u);

            }

            foreach (DataRow item in result2.Rows)
            {
                Category u = new Category();

                u.CategoryId = item.Field<int>("CategoryId");

                u.CategoryName = item.Field<string>("CategoryName");

                model.Categories.Add(u);

            }

            foreach (DataRow item in result3.Rows)
            {
                EventLevel u = new EventLevel();

                u.EventLevelId = item.Field<int>("EventLevelId");

                u.EventLevelName = item.Field<string>("EventLevelName");

                model.EventLevels.Add(u);

            }

            return View(model);
        }


        [HttpPost]
        public IActionResult EventAdd(EventAdd model, Images m, Category c) // parametreler eklenecek (upload.etc)
        {
            var prms = new Dictionary<string, object>();//event table
            var userId = HttpContext.Session.GetInt32("UserId");

            if (model.ImageFileData != null)
            {
                var extension = Path.GetExtension(model.ImageFileData.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Event/", newimagename);
                var Tlocation = "~/images/Event/" + newimagename;
                var stream = new FileStream(location, FileMode.Create);

                model.ImageFileData.CopyTo(stream);

                prms.Add("FileWay", Tlocation);
                prms.Add("ImageFileName", m.ImageFileName);

                prms.Add("CategoryId", model.CategoryId);
                prms.Add("CreatedByUserId", userId);

                //forURL.Add("IntroductionVideoLink", model.IntroductionVideoLink);

                prms.Add("EventsName", model.EventsName);
                prms.Add("EventsDescription", model.EventsDescription);

                prms.Add("EventTypesId", model.EventTypesId);
                prms.Add("EndDate", model.EndDate);//datetime convert nvarchar hatası veriyor...
                prms.Add("StartDate", model.StartDate);
                //prms.Add("ImageId", Convert.ToInt32(model.ImageId));
                prms.Add("EventLevelId", model.EventLevelId);
                prms.Add("ParticipantNumber", model.ParticipantNumber);
                prms.Add("Hours", model.Hours);
                prms.Add("IsItFree", model.IsItFree);



                //forCategory.Add("EventCategories", model.EventCategories);

                var addEvent = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Event", prms);// prosedürde image'i de kaydet:


            }

            //var addCategory = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_EventCategories", forCategory);
            TempData["msg"] = "Event is Created Successfully";
            return Redirect("/AfterLogin/home");

        }

        [HttpPost]
        public IActionResult Update(EventAdd model, Images m, Category c) // parametreler eklenecek (upload.etc)
         {
            var prms = new Dictionary<string, object>();//event table
            var userId = HttpContext.Session.GetInt32("UserId");

            if (model.ImageFileData != null)
            {
                var extension = Path.GetExtension(model.ImageFileData.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Event/", newimagename);
                var Tlocation = "~/images/Event/" + newimagename;
                var stream = new FileStream(location, FileMode.Create);

                model.ImageFileData.CopyTo(stream);

                prms.Add("ImageFileName", m.ImageFileName);
                prms.Add("FileWay", Tlocation);

               

                //forURL.Add("IntroductionVideoLink", model.IntroductionVideoLink);

                


            }
            prms.Add("EventsId", model.EventsId);
            prms.Add("ImageId", model.ImageId);
            prms.Add("CreatedByUserId", userId);
            prms.Add("CategoryId", model.CategoryId);
            prms.Add("EventsName", model.EventsName);
            prms.Add("EventsDescription", model.EventsDescription);

            prms.Add("EventTypesId", model.EventTypesId);
            prms.Add("EndDate", model.EndDate);//datetime convert nvarchar hatası veriyor...
            prms.Add("StartDate", model.StartDate);
            //prms.Add("ImageId", Convert.ToInt32(model.ImageId));
            prms.Add("EventLevelId", model.EventLevelId);
            prms.Add("ParticipantNumber", model.ParticipantNumber);
            prms.Add("Hours", model.Hours);
            prms.Add("IsItFree", model.IsItFree);
            prms.Add("IsActive", model.IsActive);



            //forCategory.Add("EventCategories", model.EventCategories);

            var addEvent = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Event", prms);// prosedürde image'i de kaydet:

            //var addCategory = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_EventCategories", forCategory);
            TempData["msg"] = "Event is Update Successfully";
            return Redirect("/Event/DetailsEvent");

        }

        [HttpPost]
        public IActionResult DetailEvent(int id)
        {
            EventModel m = new EventModel();
            var prms = new Dictionary<string, object>();
            prms.Add("EventsId", id);

            var slider = DataHelper.ListFromStoredProcedure("Sp_SelectImageFileNameFrom_Events", prms);
            foreach (DataRow item in slider.Rows)
            {
                m.Images.ImageId = item.Field<int>("ImageId");
                m.Eventmodel.CategoryName = item.Field<string>("CategoryName");
                m.Eventmodel.EventTypesName = item.Field<string>("EventTypesName");
                m.Eventmodel.EventLevelName = item.Field<string>("EventLevelName");
                m.Images.FileWay = item.Field<string>("FileWay");
                m.Images.ImageFileName = item.Field<string>("ImageFileName");
                m.Eventmodel.EventsName = item.Field<string>("EventsName");
                m.Eventmodel.EventsDescription = item.Field<string>("EventsDescription");
                m.Eventmodel.EndDate = item.Field<string>("EndDate").Replace("T", " ");
                m.Eventmodel.StartDate = item.Field<string>("StartDate").Replace("T", " ");
                m.Eventmodel.IsItFree = item.Field<bool>("IsItFree");
                m.Eventmodel.EventsId = item.Field<int>("EventsId");
                m.Eventmodel.ParticipantNumber = item.Field<int>("ParticipantNumber");
                m.Eventmodel.UserName = item.Field<string>("FirstName");
            }
            return View(m);
        }

        public IActionResult DetailsEvent()
        {
            EventModel model = new EventModel();
            var prms = new Dictionary<string, object>();
            var slider = DataHelper.ListFromStoredProcedure("Sp_SelectImageFileNameFrom_Events", prms);
            foreach (DataRow item in slider.Rows)
            {
                EventModel m = new EventModel();
                m.Images.ImageId = item.Field<int>("ImageId");
                m.Eventmodel.CategoryName = item.Field<string>("CategoryName");
                m.Eventmodel.EventTypesName = item.Field<string>("EventTypesName");
                m.Eventmodel.EventLevelName = item.Field<string>("EventLevelName");
                m.Images.FileWay = item.Field<string>("FileWay");
                m.Images.ImageFileName = item.Field<string>("ImageFileName");
                m.Eventmodel.EventsName = item.Field<string>("EventsName");
                m.Eventmodel.EventsDescription = item.Field<string>("EventsDescription");
                m.Eventmodel.EndDate = item.Field<string>("EndDate").Replace("T", " ");
                m.Eventmodel.StartDate = item.Field<string>("StartDate").Replace("T", " ");
                m.Eventmodel.IsItFree = item.Field<bool>("IsItFree");
                m.Eventmodel.EventsId = item.Field<int>("EventsId");
                m.Eventmodel.ParticipantNumber = item.Field<int>("ParticipantNumber");
                m.Eventmodel.UserName = item.Field<string>("FirstName");
                model.EventList.Add(m);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateEvent(int id)
        {
            
            EventModel m = new EventModel();
            var createEvent = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_EventTypes", createEvent);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_Display_Categories", createEvent);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_Display_EventLevel", createEvent);
            var forID2 = new Dictionary<string, object>();
            var forNameId = new Dictionary<string, object>();
            var prms = new Dictionary<string, object>();
            forID2.Add("UserEmail", HttpContext.Session.GetString("UserEmail"));

            var userid = DataHelper.GetFromStoredProcedure<int>("Sp_SelectIdByMail_User", forID2);
            HttpContext.Session.SetInt32("UserId", userid);
            forNameId.Add("UserID", userid);
            var userName = DataHelper.GetFromStoredProcedure<string>("Sp_DisplayUserById_User", forNameId);
            HttpContext.Session.SetString("UserName", userName);
            foreach (DataRow item in result1.Rows)
            {
                EventType u = new EventType();

                u.EventTypesId = item.Field<int>("EventTypesId");

                u.EventTypesName = item.Field<string>("EventTypesName");

                m.EventTypes.Add(u);

            }

            foreach (DataRow item in result2.Rows)
            {
                Category u = new Category();

                u.CategoryId = item.Field<int>("CategoryId");

                u.CategoryName = item.Field<string>("CategoryName");

                m.Categories.Add(u);

            }

            foreach (DataRow item in result3.Rows)
            {
                EventLevel u = new EventLevel();

                u.EventLevelId = item.Field<int>("EventLevelId");

                u.EventLevelName = item.Field<string>("EventLevelName");

                m.EventLevels.Add(u);

            }

            //fetch data with incoming id
            prms.Add("EventsId", id);
            
            var slider = DataHelper.ListFromStoredProcedure("Sp_SelectImageFileNameFrom_Events", prms);
            foreach (DataRow item in slider.Rows)
            {
                m.Images.ImageId = item.Field<int>("ImageId");
                m.Eventmodel.CategoryName = item.Field<string>("CategoryName");
                m.Eventmodel.EventTypesName = item.Field<string>("EventTypesName");
                m.Eventmodel.EventLevelName = item.Field<string>("EventLevelName");
                m.Images.FileWay = item.Field<string>("FileWay");
                m.Images.ImageFileName = item.Field<string>("ImageFileName");
                m.Eventmodel.EventsName = item.Field<string>("EventsName");
                m.Eventmodel.EventsDescription = item.Field<string>("EventsDescription");
                m.Eventmodel.EndDate = item.Field<string>("EndDate").Replace("T", " ");
                m.Eventmodel.StartDate = item.Field<string>("StartDate").Replace("T", " ");
                m.Eventmodel.IsItFree = item.Field<bool>("IsItFree");
                m.Eventmodel.EventsId = item.Field<int>("EventsId");
                m.Eventmodel.ParticipantNumber = item.Field<int>("ParticipantNumber");
                m.Eventmodel.IsActive = item.Field<bool>("IsActive");
                m.Eventmodel.UserName = item.Field<string>("FirstName");
            }
            return View(m);
        }

        [HttpPost]
        public IActionResult JoinEvent(int id)
        {

            var prms = new Dictionary<string, object>();
            prms.Add("UserId", HttpContext.Session.GetInt32("UserId"));
            prms.Add("EventId", id);
            var slider = DataHelper.ListFromStoredProcedure("Sp_InsertUpdate_EventParticipantList", prms);
            return Redirect("/AfterLogin/home");
        }
    }
}