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
    public class ProfileUpdateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Update()
        {
            ProfileUpdateModel model = new ProfileUpdateModel();

            var prms = new Dictionary<string, object>();
            var userId = HttpContext.Session.GetInt32("UserId");
            prms.Add("UserId", userId);

            var userInformation = DataHelper.ListFromStoredProcedure("Sp_Display_User", prms);

            getSelectOption(model);

            Update updateModel = new Update();
            foreach (DataRow item in userInformation.Rows)
            {
                model.UpdateModel.Name = item.Field<string>("FirstName");
                model.UpdateModel.Surname = item.Field<string>("LastName");
                model.UpdateModel.Email = item.Field<string>("UserEmail");
                model.UpdateModel.AreaCodeId = item.Field<int>("AreaCodeId");
                model.UpdateModel.Phone = item.Field<string>("TelephoneNumber");
                model.UpdateModel.UniversityId = item.Field<int>("UniversityId");
                model.UpdateModel.DepartmantId = item.Field<int>("DepartmentId");
                model.UpdateModel.ClassId = item.Field<int>("ClassId");
            }
            var userSkills = DataHelper.ListFromStoredProcedure("Sp_Display_UserSkills", prms);

            foreach (DataRow item in userSkills.Rows)
            {
                model.UpdateModel.UserSkills.Add(item.Field<int>("SkillId"));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(Update model, IFormFile UploadCV, IFormFile UploadLogo)
        {
            var prms = new Dictionary<string, object>();
            var prms2 = new Dictionary<string, object>();
            
            var userId = HttpContext.Session.GetInt32("UserId");

            prms.Add("UserId", userId);
            
            prms.Add("FirstName", model.Name);

            prms.Add("LastName", model.Surname);

            if (model.ConfirmEmail != null) 
                prms.Add("UserEmail", model.ConfirmEmail);

            if (model.ConfirmPassword != null)
                prms.Add("UserPassword", model.ConfirmPassword);

            if (model.ConfirmPhone != null) 
                prms.Add("TelephoneNumber", model.ConfirmPhone);

            if (model.ConfirmAreaCodeId > 0)
                prms.Add("AreaCodeId", model.ConfirmAreaCodeId);

            prms.Add("UniversityId", model.UniversityId);

            prms.Add("DepartmentId", model.DepartmantId);

            prms.Add("ClassId", model.ClassId);

            ProfileUpdateModel pModel = new ProfileUpdateModel();

            getSelectOption(pModel);

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

            var a = DataHelper.RunFromStoredProcedure("Sp_InsertUpdate_User", prms);

            var prms3 = new Dictionary<string, object>();
            prms3.Add("UserId", userId);
            prms3.Add("ModifiedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            var b = DataHelper.RunFromStoredProcedure("Sp_Update_UserSkills", prms3);

            prms3.Add("CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            
            foreach (var x in model.UserSkills)
            {
                prms3.Add("SkillId", x);
                var c = DataHelper.RunFromStoredProcedure("Sp_Insert_UserSkills", prms3);
                prms3.Remove("SkillId");
            }

            prms2.Add("UserId", userId);

            var userInformation = DataHelper.ListFromStoredProcedure("Sp_Display_User", prms2);

            foreach (DataRow item in userInformation.Rows)
            {
                pModel.UpdateModel.Name = item.Field<string>("FirstName");
                pModel.UpdateModel.Surname = item.Field<string>("LastName");
                pModel.UpdateModel.Email = item.Field<string>("UserEmail");
                pModel.UpdateModel.AreaCodeId = item.Field<int>("AreaCodeId");
                pModel.UpdateModel.Phone = item.Field<string>("TelephoneNumber");
                pModel.UpdateModel.UniversityId = item.Field<int>("UniversityId");
                pModel.UpdateModel.DepartmantId = item.Field<int>("DepartmentId");
                pModel.UpdateModel.ClassId = item.Field<int>("ClassId");
            }

            var userSkills = DataHelper.ListFromStoredProcedure("Sp_Display_UserSkills", prms2);

            foreach (DataRow item in userSkills.Rows)
            {
                pModel.UpdateModel.UserSkills.Add(item.Field<int>("SkillId"));
            }
             return View(pModel);
        }

        public void getSelectOption(ProfileUpdateModel model)
        {
            var result1 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_University");
            var result2 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_Class");
            var result3 = DataHelper.ListFromStoredProcedure("Sp_SelectAll_Department");
            var result4 = DataHelper.ListFromStoredProcedure("Sp_Select_AreaCode");
            var result5 = DataHelper.ListFromStoredProcedure("Sp_Display_Skills");
            //var result4 = DataHelper.ListFromStoredProcedure("Sp_Country", prms);
            //var result5 = DataHelper.ListFromStoredProcedure("Sp_City", prms);
            //var result6 = DataHelper.ListFromStoredProcedure("Sp_Town", prms);
            foreach (DataRow item in result1.Rows)
            {
                University u = new University();
                u.UniversityId = item.Field<int>("UniversityId");
                u.UniversityName = item.Field<string>("UniversityName");
                model.Universities.Add(u);
            }
            foreach (DataRow item in result2.Rows)
            {
                Class u = new Class();
                u.ClassId = item.Field<int>("ClassId");
                u.ClassName = item.Field<string>("ClassName");
                model.Classes.Add(u);
            }
            foreach (DataRow item in result3.Rows)
            {
                Departmant u = new Departmant();
                u.DepartmantId = item.Field<int>("DepartmantId");
                u.DepartmantName = item.Field<string>("DepartmantName");
                model.Departmants.Add(u);
            }
            foreach (DataRow item in result4.Rows)
            {
                AreaCode u = new AreaCode();
                u.AreaCodeId = item.Field<int>("AreaCodeId");
                u.AreaCodeName = item.Field<string>("AreaCodeName");
                model.AreaCodes.Add(u);

            }
            foreach (DataRow item in result5.Rows)
            {
                Skills u = new Skills();
                u.SkillsId = item.Field<int>("SkillId");
                u.SkillsName = item.Field<string>("SkillName");
                model.Skills.Add(u);

            }
            //foreach (DataRow item in result4.Rows)
            //{
            //    Country u = new Country();
            //    u.CountryId = item.Field<int>("CountryId");
            //    u.CountryName = item.Field<string>("CountryName");
            //    model.Countries.Add(u);
            //}
            //foreach (DataRow item in result5.Rows)
            //{
            //    City u = new City();
            //    u.CityId = item.Field<int>("CityId");
            //    u.CityName = item.Field<string>("CityName");
            //    model.Cities.Add(u);
            //}
            //foreach (DataRow item in result6.Rows)
            //{
            //    Town u = new Town();
            //    u.TownId = item.Field<int>("TownId");
            //    u.TownName = item.Field<string>("TownName");
            //    model.Towns.Add(u);
            //}
        }
    }
}