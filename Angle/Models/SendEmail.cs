using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;



namespace Angle.Models
{
    public class SendEmail
    {
        public SendEmail()
        {
            this.Roles = new List<Role>();
            this.EventTypes = new List<EventType>();
            this.Categories = new List<Category>();

            this.EventList = new List<SendEmail>();

        }
        public String mail { get; set; }



        public String subject { get; set; }




        public List<Role> Roles { get; set; }
        public List<EventType> EventTypes { get; set; }
        public List<Category> Categories { get; set; }

        public List<SendEmail> EventList { get; set; }
        public MailAddress From { get; internal set; }
        public MailAddress To { get; internal set; }

    }
}