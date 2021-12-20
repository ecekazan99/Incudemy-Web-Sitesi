using Angle.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class EventModel
    {
        

        public EventModel()
        {
            this.Eventmodel = new EventAdd();
            this.EventList = new List<EventModel>();
            this.EventTypes = new List<EventType>();
            this.Categories = new List<Category>();
            this.EventLevels = new List<EventLevel>();
            this.Images = new Images();
            this.Slider = new SliderSettingsModel();
            
        }
        
        public SliderSettingsModel Slider { get; set; }
        public EventAdd Eventmodel { get; set; }
        public List<EventModel> EventList { get; set; }
        public List<EventType> EventTypes { get; set; }
        public List<Category> Categories { get; set; }
        public Images Images { get; set; }
        public List<EventLevel> EventLevels { get; set; }

    }
}
