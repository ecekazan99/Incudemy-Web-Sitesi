using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace Angle.Models
{
    public class LoginModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Email is required!")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
      public string UserName { get; set; }
        public MailAddress From { get; internal set; }
        public MailAddress To { get; internal set; }
        public String subject { get; set; }

    }

}