using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Angle.Models;
using Angle.Helpers;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Angle.Controllers
{
    public class ListUserController : Controller
    {
        public IActionResult listUser()
        {
            FilterUserModel model = new FilterUserModel();



            var filter = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_Display_EventTypes", filter);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_Display_Categories", filter);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_Display_Role", filter);
            var result4 = DataHelper.ListFromStoredProcedure("Sp_Display_Class", filter);
            var result5 = DataHelper.ListFromStoredProcedure("Sp_FilterUser_User", filter);

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
                Role u = new Role();

                u.RoleId = item.Field<int>("RoleId");

                u.UserRoleName = item.Field<string>("UserRoleName");

                model.Roles.Add(u);

            }
            foreach (DataRow item in result4.Rows)
            {
                Class u = new Class();

                u.ClassId = item.Field<int>("ClassId");

                u.ClassName = item.Field<string>("ClassName");

                model.Classes.Add(u);

            }
            //foreach (DataRow item in result5.Rows)
            //{
            //    UserAdd u = new UserAdd();

            //    u.FirstName = item.Field<string>("FirstName");
            //    u.Surname = item.Field<string>("LastName");
            //    u.Email = item.Field<string>("UserEmail");

            //    model.Users.Add(u);

            //}

            return View(model);
        }

        /*  [HttpPost]
          public JsonResult FilterUserList(IFormCollection collection)
          {
              var filter = collection.Keys.ToDictionary(a => a, b => collection[b] as object);
              var result = DataHelper.ListFromStoredProcedure("Sp_FilterUser_User", null);
              // list object sekilde çevirip böyle deneyin 
              //var jsonResult = JsonConvert.SerializeObject(result);

              return Json(result);
          }*/
        [HttpPost]
        public JsonResult FilterUserList(IFormCollection collection)
        {
            var prms = collection.Keys.ToDictionary(a => a, b => collection[b] as object);
            //var prms = new Dictionary<string, object>();
            Dictionary<string, object> dict = new Dictionary<string, object>();

            //prms.Add("EventTypeId", filterUserAdd.EventTypeId);
            foreach (var item in prms)
            {
                dict.Add(item.Key, item.Value);
            }

            DataTable result = DataHelper.ListFromStoredProcedure("Sp_FilterUser_User", dict);

            var jsonResult = JsonConvert.SerializeObject(result);

            //var deSerializejsonResult = JsonConvert.DeserializeObject(jsonResult);
            
            return Json(jsonResult);
        }
    }
}
