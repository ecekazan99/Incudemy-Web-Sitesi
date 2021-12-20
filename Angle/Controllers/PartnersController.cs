using Angle.Helpers;
using Angle.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Angle.Controllers
{
    public class PartnersController : Controller
    {
        //private readonly IWebHostEnvironment _env;
        //public PartnersController(IWebHostEnvironment env)
        //{
        // _env = env;
        //}
        public IActionResult PartnersHome()
        {
            PartnersModel model = new PartnersModel();
            var partners = new Dictionary<string, object>();
            //var userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            // partners.Add("UserId", userId);
            var partnersImages = DataHelper.ListFromStoredProcedure("Sp_Display_Partner", partners);

            
             foreach (DataRow item in partnersImages.Rows)
            {
                Images u = new Images();
                //Partners p = new Partners();                 //u.ImageId = item.Field<int>("ImageId");
                //u.ImageFileName = item.Field<string>("ImageFileName");
                //u.FileWay = item.Field<string>("FileWay");                 //p.PartnerId = item.Field<int>("PartnerId");
                //p.Title = item.Field<string>("Title");
                //p.Description = item.Field<string>("Description"); 
                //model.ImagesList.Add(u);                 //model.PartnerList.Add(p);
                PartnersModel partnersModel = new PartnersModel();
                partnersModel.ImageId = item.Field<int>("ImageId");
                partnersModel.ImageFileName = item.Field<string>("ImageFileName");
                partnersModel.File = item.Field<string>("FileWay");
                partnersModel.ParnerId = item.Field<int>("PartnerId");
                partnersModel.Title = item.Field<string>("Title");
                partnersModel.Description = item.Field<string>("Description");
                model.ImagesList.Add(u);
                model.Allpartners.Add(partnersModel);
            }
            return View(model);
        }
        public IActionResult PartnersIndex()
        {
            PartnersModel model = new PartnersModel();
            var partners = new Dictionary<string, object>();


            var partnersImages = DataHelper.ListFromStoredProcedure("Sp_Display_Partner", partners);

            //var partnersImages = DataHelper.ListFromStoredProcedure("Sp_DisplayImageIdByUserId_User", partners);


            

            foreach (DataRow item in partnersImages.Rows)
            {
                Images u = new Images();                 //Partners p = new Partners();
                PartnersModel partnersModel = new PartnersModel();                 //u.ImageId = item.Field<int>("ImageId");
                //u.ImageFileName = item.Field<string>("ImageFileName");
                //u.FileWay = item.Field<string>("FileWay"); 
                ////p.PartnerId = item.Field<int>("PartnerId");
                //p.Title = item.Field<string>("Title");
                //p.Description = item.Field<string>("Description");                 //model.ImagesList.Add(u);                 //model.PartnerList.Add(p);
                partnersModel.ImageId = item.Field<int>("ImageId");
                partnersModel.ImageFileName = item.Field<string>("ImageFileName");
                partnersModel.File = item.Field<string>("FileWay");

                partnersModel.ParnerId = item.Field<int>("PartnerId");
                partnersModel.Title = item.Field<string>("Title");
                partnersModel.Description = item.Field<string>("Description");
                model.ImagesList.Add(u);
                model.Allpartners.Add(partnersModel);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Partners()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Partners(PartnersModel model)
        {
            Images images = new Images();
            Partners partners = new Partners();
            var prms = new Dictionary<string, object>();
            var pr = new Dictionary<string, object>();
            if (model.FileWay != null)
            {
                var extension = Path.GetExtension(model.FileWay.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Partner/", newimagename);
                var Tlocation = "~/images/Partner/" + newimagename;
                var stream = new FileStream(location, FileMode.Create);
                prms.Add("FileWay", Tlocation); model.FileWay.CopyTo(stream); images.FileWay = Tlocation;
                images.ImageFileName = model.ImageFileName; partners.Title = model.Title;
                partners.Description = model.Description; images.FileWay = location;
                images.ImageFileName = model.ImageFileName;
            }
            model.ImageFileName = images.ImageFileName;
            model.ImageId = images.ImageId; model.ParnerId = partners.PartnerId;
            model.ImageId = partners.ImageId;
            model.Title = partners.Title;
            model.Description = partners.Description;
            prms.Add("Title", model.Title);
            prms.Add("Description", model.Description);

            prms.Add("ImageFileName", model.ImageFileName);
            var Id = (HttpContext.Session.GetInt32("UserId"));
            prms.Add("UserId", Id);




            // var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);
            //prms.Add("FileWay", images.FileWay);          


            //var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);


            //var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);

            var b = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Partners", prms);

            prms.Add("Description", model.Description);
            prms.Add("ImageFileName", model.ImageFileName);
            prms.Add("UserId", Id);

            // var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);
            //prms.Add("FileWay", images.FileWay);          

            //var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);             
            //var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_Image", prms);
            return Redirect("PartnersIndex");
        }
        //private async Task<string> UploadDocument(IFormFile file)
        //{
        // string folderpath = null;
        // folderpath += Guid.NewGuid().ToString() + "_" + file.FileName;
        // string serverFolder = Path.Combine(_env.WebRootPath, folderpath);
        // var f = new FileStream(serverFolder, FileMode.Create);
        // await file.CopyToAsync(f);
        // f.Close();
        // return serverFolder;
        //}
    }
}