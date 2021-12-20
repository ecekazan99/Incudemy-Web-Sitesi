using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
   
    public class PartnersModel
    {
        public int ParnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int ImageId { get; set; }
        public string ImageFileName { get; set; }
        public string File { get; set; }
        public IFormFile FileWay { get; set; }
        public PartnersModel()
        {
            this.Partnersmodel = new Images();
            this.ImagesList = new List<Images>();
            this.P = new Partners();
            this.PartnerList = new List<Partners>();
            this.Allpartners = new List<PartnersModel>();
        }
        public List<PartnersModel> Allpartners { get; set; }
        public Images Partnersmodel { get; set; }
        public List<Images> ImagesList { get; set; }
        public Partners P { get; set; }
        public List<Partners> PartnerList { get; set; }
    }
}
