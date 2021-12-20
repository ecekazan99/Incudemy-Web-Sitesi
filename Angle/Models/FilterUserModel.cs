using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class FilterUserModel
    {
        public FilterUserModel()
        {
            this.EventTypes = new List<EventType>();
            this.Categories = new List<Category>();
            this.Roles = new List<Role>();
            this.Classes = new List<Class>();
            this.Users = new List<UserAdd>();
            this.Eventmodel = new EventAdd();
            this.EventList = new List<FilterUserModel>();
        }
        public List<Role> Roles { get; set; } 
        public List<EventType> EventTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<Class> Classes { get; set; }
        public List<UserAdd> Users { get; set; }
        public List<FilterUserModel> EventList { get; set; }
        public EventAdd Eventmodel { get; set; }
    }
}
