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
using Microsoft.Extensions.Logging;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Angle.Controllers
{
    public class SliderSettingsController : Controller
    {

        public IActionResult SliderSettings()
        {
            SliderSettingsModel model = new SliderSettingsModel();


            return View(model);
        }

        public IActionResult SliderList()   //Yüklediğimiz fotoğrafları, sırayı ve tarihi listeleyip görebileceğimiz sayfa. Databaseden veri cekme.
        {
            SliderSettingsModel model = new SliderSettingsModel();
            var sliderSetting = new Dictionary<string, object>();

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
                u.IsActive = item.Field<bool>("IsActive");
                model.PageImagesList.Add(u);
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult DetailSlider(int? id)
        {
            SliderSettingsModel m = new SliderSettingsModel();
            var prms = new Dictionary<string, object>();
            prms.Add("PageImagesId", id);
            var slider = DataHelper.ListFromStoredProcedure("sp_SelectPageImagebyPamgeImagesId_PageImage", prms);

            foreach (DataRow item in slider.Rows)
            {
                m.Images.ImageId = item.Field<int>("ImageId");
                m.Images.FileWay = item.Field<string>("FileWay");
                m.Images.ImageFileName = item.Field<string>("ImageFileName");
                m.PageImagesId = item.Field<int>("PageImagesId");
                m.SortOrder = item.Field<int>("SortOrder");
                m.CreatedDate = item.Field<DateTime>("CreatedDate");
                m.SiteMenuId = item.Field<int>("SiteMenuId");
                m.IsActive = item.Field<bool>("IsActive");
            }

            return View(m);
        }

        public IActionResult UpdateSlider(SliderSettingsModel model)
        {
            var prms = new Dictionary<string, object>();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (model.File != null)
            {
                var extension = Path.GetExtension(model.File.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Slider/", newimagename);
                var Tlocation = "~/images/Slider/" + newimagename;
                var stream = new FileStream(location, FileMode.Create);

                model.File.CopyTo(stream);


                prms.Add("FileWay", Tlocation);
                prms.Add("ImageFileName", model.Images.ImageFileName);

                


            }
            prms.Add("PageImagesId", model.PageImagesId);
            prms.Add("SiteMenuId", 1);//will be added to afterloginpage by default.

            prms.Add("SortOrder", model.SortOrder);
            prms.Add("IsActive", model.IsActive);
            var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_PageImages", prms);

            return Redirect("SliderList");
        }


        [HttpPost]
        public IActionResult SliderSettings(SliderSettingsModel model)
        {
            var prms = new Dictionary<string, object>();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (model.File != null)
            {
                var extension = Path.GetExtension(model.File.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Slider/", newimagename);
                var Tlocation = "~/images/Slider/" + newimagename;
                var stream = new FileStream(location, FileMode.Create);

                model.File.CopyTo(stream);


                prms.Add("FileWay", Tlocation);
                prms.Add("ImageFileName", model.Images.ImageFileName);

                prms.Add("CreatedByUserId", userId);
                prms.Add("SiteMenuId", 1);//will be added to afterloginpage by default.

                prms.Add("SortOrder", model.SortOrder);


            }

            var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_PageImages", prms);


            return Redirect("SliderList");


        }
    }
}