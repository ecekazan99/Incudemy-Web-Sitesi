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



namespace Angle.Controllers
{
    public class AfterLoginController : Controller
    {
        EventModel model = new EventModel();

        public IActionResult Home(EventType search, Category search2)
        {

            var prms = new Dictionary<string, object>();
            var activeEventList = new Dictionary<string, object>();
            activeEventList.Add("IsActive", true);
            //var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_Event", prms);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_Display_EventTypes", prms);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_Display_Categories", prms);
            var result4 = DataHelper.ListFromStoredProcedure("Sp_Display_EventLevel", prms);
            var result1 = DataHelper.ListFromStoredProcedure("Sp_SelectImageFileNameFrom_Events", activeEventList);

            var sliderSetting = new Dictionary<string, object>();
            sliderSetting.Add("IsActive", 1);
            var sliderOrder = DataHelper.ListFromStoredProcedure("Sp_DisplayPageImages_Image", sliderSetting);

            foreach (DataRow item in sliderOrder.Rows)
            {
                SliderSettingsModel u = new SliderSettingsModel();
                u.Images.ImageId = item.Field<int>("ImageId");
                u.Images.FileWay = item.Field<string>("FileWay");
                u.Images.ImageFileName = item.Field<string>("ImageFileName");
                u.PageImagesId = item.Field<int>("PageImagesId");
                u.SortOrder = item.Field<int>("SortOrder");
                u.CreatedDate = item.Field<DateTime>("CreatedDate");
                u.SiteMenuId = item.Field<int>("SiteMenuId");
                model.Slider.PageImagesList.Add(u);
            }



            foreach (DataRow item in result2.Rows)
            {
                EventType u = new EventType();

                //u.EventTypesId = item.Field<int>("EventTypesId");

                //u.EventTypesName = item.Field<string>("EventTypesName");

                model.EventTypes.Add(u);

            }
            foreach (DataRow item in result3.Rows)
            {
                Category u = new Category();

                u.CategoryId = item.Field<int>("CategoryId");

                u.CategoryName = item.Field<string>("CategoryName");

                model.Categories.Add(u);

            }
            foreach (DataRow item in result4.Rows)
            {
                EventLevel u = new EventLevel();

                //u.EventLevelId = item.Field<int>("EventLevelId");

                u.EventLevelName = item.Field<string>("EventLevelName");

                model.EventLevels.Add(u);

            }


            foreach (DataRow item in result1.Rows)

            {

                EventModel u = new EventModel();

                u.Eventmodel.EventsId = item.Field<int>("EventsId");

                u.Eventmodel.CreatedByUserId = item.Field<int>("CreatedByUserId");

                u.Eventmodel.EventsName = item.Field<string>("EventsName");

                //  u.Eventmodel.EventCategories = item.Field<string>("EventCategories");

                //u.Eventmodel.EventLevelId = item.Field<int>("EventLevelId");

                //u.Eventmodel.EventTypesId = item.Field<int>("EventTypesId");

                u.Eventmodel.ImageId = item.Field<int>("ImageId");

                //u.Eventmodel.ImageId = item.Field<string>("ImageId");


                u.Eventmodel.IsItFree = item.Field<bool>("IsItFree");

                u.Eventmodel.ParticipantNumber = item.Field<int>("ParticipantNumber");

                //u.Eventmodel.Hours = item.Field<int>("Hours");

                u.Eventmodel.StartDate = item.Field<string>("StartDate").Replace("T", " ");

                u.Eventmodel.EndDate = item.Field<string>("EndDate").Replace("T", " ");

                //u.Eventmodel.ImageId = item.Field<string>("ImageId");

                u.Eventmodel.EventsDescription = item.Field<string>("EventsDescription");
                u.Images.FileWay = item.Field<string>("FileWay");
                u.Images.ImageFileName = item.Field<string>("ImageFileName");


                model.EventList.Add(u);


            }


            /*if (search != null && search2 != null)
             {
                 model.EventList.Where(x => x.EventTypes.Contains(search) && x.Categories.Contains(search2)).ToList();

             }*/
            return View(model);
        }
        [HttpPost]
        public IActionResult EventFilter(EventAdd model)
        {
            var prms = new Dictionary<string, object>();
            prms.Add("EventTypesId", model.EventTypesId);
            prms.Add("EventCategories", model.CategoryId);
            var filter = DataHelper.ListFromStoredProcedure("Sp_FilterByTypeOrCategory_Event", prms);



            return Redirect("/AfterLogin/Home");
        }
    }
}
