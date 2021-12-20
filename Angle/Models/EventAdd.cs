using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Angle.Models
{
    public class EventAdd
    {
        public int EventsId { get; set; }

        [Required(ErrorMessage = "User ID is required!")]
        public int CreatedByUserId { get; set; }

        [Required(ErrorMessage = "Event Name is required!")]
        [MaxLength(100, ErrorMessage = "Event name must be max 500 char!")]
        public string EventsName { get; set; }

        /* [Required(ErrorMessage = "Instructor name is required!")]
         public string IntroductionName { get; set; }*/


        [Required(ErrorMessage = "Event Type is required!")]
        public int EventTypesId { get; set; }

        [Required(ErrorMessage = "Event Categories is required!")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Is it Free is required!")]
        public bool IsItFree { get; set; }

        //  [Required(ErrorMessage = "Event Level is required!")]
        public int EventLevelId { get; set; }

        [Required(ErrorMessage = "Maximum Participant is required!")]
        public int ParticipantNumber { get; set; }

        [Required(ErrorMessage = "Starting Date is required!")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string StartDate { get; set; }


        [Required(ErrorMessage = "Ending Date is required!")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string EndDate { get; set; }


        [Required(ErrorMessage = "Hour is required!")]
        public int Hours { get; set; }

        public int ImageId { get; set; }

        public IFormFile ImageFileData { get; set; }

        [Required(ErrorMessage = "Event Description is required!")]
        [MaxLength(500,ErrorMessage ="Event Description must be max 500 char!")]
        public string EventsDescription { get; set; }

        public string CategoryName { get;set; }
        public string EventLevelName { get;  set; }
        public string EventTypesName { get;  set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
    }

}