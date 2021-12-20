using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Angle.Models
{
    public class Images
    {
        
        public int ImageId { get; set; }

        [MaxLength(100, ErrorMessage = "Event name must be max 500 char!")]
        public string ImageFileName { get; set; }
        
        public string FileWay { get; set; }
        
    }
}
