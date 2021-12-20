using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class ProfileUpdateModel
    {
        public string GetUserName()
        {
            Users name = new Users();
            string username = name.Username;
            return username;

        }
        public ProfileUpdateModel()
        {
            this.UpdateModel = new Update();
            this.AreaCodes = new List<AreaCode>();
            this.Universities = new List<University>();
            this.Departmants = new List<Departmant>();
            this.Classes = new List<Class>();
            this.Countries = new List<Country>();
            this.Cities = new List<City>();
            this.Towns = new List<Town>();
            this.Skills = new List<Skills>();
        }
        public Update UpdateModel { get; set; }
        public List<AreaCode> AreaCodes { get; set; }
        public List<University> Universities { get; set; }
        public List<Departmant> Departmants { get; set; }
        public List <Class>Classes { get; set; }
        public List<Skills> Skills { get; set; }
        public List<Country> Countries { get; set; }
        public List<City> Cities { get; set; }
        public List<Town> Towns { get; set; }
    }
}
