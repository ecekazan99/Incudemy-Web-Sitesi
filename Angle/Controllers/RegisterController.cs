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
using System.IO;

namespace Angle.Controllers
{
    public class RegisterController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Useradd()
        {
            RegisterModel model = new RegisterModel();

            var prms = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_University", prms);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_Department", prms);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_Class", prms);
            var result4 = DataHelper.ListFromStoredProcedure("Sp_Select_AreaCode", prms);

            foreach (DataRow item in result1.Rows)
            {
                University u = new University();
                u.UniversityId = item.Field<int>("UniversityId");
                u.UniversityName = item.Field<string>("UniversityName");
                model.Universities.Add(u);
            }
            foreach (DataRow item in result2.Rows)
            {
                Departmant u = new Departmant();
                u.DepartmantId = item.Field<int>("DepartmantId");
                u.DepartmantName = item.Field<string>("DepartmantName");
                model.Departmants.Add(u);
            }
            foreach (DataRow item in result3.Rows)
            {
                Class u = new Class();
                u.ClassId = item.Field<int>("ClassId");
                u.ClassName = item.Field<string>("ClassName");
                model.Classes.Add(u);
            }
            foreach (DataRow item in result4.Rows)
            {
                AreaCode u = new AreaCode();
                u.AreaCodeId = item.Field<int>("AreaCodeId");
                u.AreaCodeName = item.Field<string>("AreaCodeName");
                model.AreaCodes.Add(u);
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Useradd(UserAdd model, IFormFile UploadCV, IFormFile UploadLogo)//sp de Identity ayarlanacak...
        {
            var prms = new Dictionary<string, object>();
            var emailValidator = new Dictionary<string, object>();
            emailValidator.Add("UserEmail", model.Email);
            var TelephoneValidator = new Dictionary<string, object>();
            TelephoneValidator.Add("TelephoneNumber", model.Phone);
            var emailReturn = DataHelper.GetFromStoredProcedure<string>("Sp_DisplayUserByEmail_User", emailValidator);
            var phoneReturn = DataHelper.GetFromStoredProcedure<string>("Sp_DisplayUserByTelephoneNumber_User", TelephoneValidator);

            if (emailReturn == null)
            {
                if (phoneReturn == null)
                {
                    if (UploadLogo != null)
                    {
                        var extension = Path.GetExtension(UploadLogo.FileName);
                        var newimagename = Guid.NewGuid() + extension;
                        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/User/", newimagename);
                        var Tlocation = "~/images/User/" + newimagename;
                        var stream = new FileStream(location, FileMode.Create);

                        UploadLogo.CopyTo(stream);
                        prms.Add("ImageFileWay", Tlocation);
                        prms.Add("ImageFileName", UploadLogo.FileName);
                    }

                    if (UploadCV != null)
                    {
                        var extension = Path.GetExtension(UploadCV.FileName);
                        var newimagename = Guid.NewGuid() + extension;
                        var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/User/", newimagename);
                        var Tlocation = "~/images/User/" + newimagename;
                        var stream = new FileStream(location, FileMode.Create);

                        UploadCV.CopyTo(stream);
                        prms.Add("PdfFileWay", Tlocation);
                        prms.Add("PdfFileName", UploadCV.FileName);
                    }

                    prms.Add("RoleId", model.role);

                    prms.Add("FirstName", model.Name);
                    prms.Add("LastName", model.Surname);
                    prms.Add("GenderId", model.GenderId);
                    prms.Add("UserEmail", model.Email);
                    prms.Add("AreaCodeId", model.AreaCodeId);
                    prms.Add("TelephoneNumber", model.Phone);
                    //prms.Add("UserRoleId", model.role);
                    prms.Add("UniversityId", model.UniversityId);
                    prms.Add("DepartmentId", model.DepartmantId);
                    prms.Add("ClassId", model.ClassId);
                    prms.Add("UserPassword", model.Password);
                    //prms.Add("UploadCV", model.UploadCV);

                    if (model.Password == model.ConfirmPassword)
                    {
                        var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_User", prms);
                        return Redirect("/Login/LoginModel");
                    }
                    else if (model.Password != model.ConfirmPassword)
                    {
                        TempData["notice"] = "**PASSWORDS DOESN'T MATCH**";
                        return Redirect("/");

                    }
                }
                else
                {
                    TempData["notice"] = "**This phone number is already in use**";
                }
            }
            else
            {
                TempData["notice"] = "**This email is already in use**";
            }
            
            return Redirect("/Register/Useradd");
        }
        [HttpPost]
        public IActionResult Student(bool hasPassport)
        {
            ViewBag.HasPassport = hasPassport;
            return View();
        }
    }
}
