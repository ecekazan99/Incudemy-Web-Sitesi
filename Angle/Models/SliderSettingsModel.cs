using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class SliderSettingsModel
    {
        public int CreatedByUserId { get; set; }

        public int SortOrder { get; set; }

        public int PageImagesId { get; set; }

        public int SiteMenuId { get; set; }

        public Images Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public IFormFile File { get; set; }

        public SliderSettingsModel()
        {

            this.Images = new Images();
            this.PageImagesList = new List<SliderSettingsModel>();

        }
        public UserAdd Usermodel { get; set; }
        public List<SliderSettingsModel> PageImagesList { get; set; }
    }
}
