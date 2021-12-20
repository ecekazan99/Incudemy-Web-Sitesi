using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Angle.Models

{
    public class GiveFeedbackModel
    {
        public GiveFeedbackModel()
        {

            this.FeedbackTypes = new List<FeedbackType>();

             
        }

        public List<FeedbackType> FeedbackTypes { get; set; }

        public String Description { get; set; }

        public int FeedbackId { get; set; }

        public int UserId { get; set; }

        public int FeedbackTypeId { get; set; }

        public int EventsId { get; set; }


    }
}

