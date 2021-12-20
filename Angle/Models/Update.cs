using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Angle.Models
{
    public class Update
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "If new e-mail is given")]
        public string ConfirmEmail { get; set; }
        public int AreaCodeId { get; set; }
        public int ConfirmAreaCodeId { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "If new phone number is given")]
        public string ConfirmPhone { get; set; }
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "If new password is given")]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Instructor { get; set; }
        public string InstructorTitle { get; set; }
        public string UploadCV { get; set; }
        public string UploadLogo { get; set; }
        public int UniversityId { get; set; }
        public int ClassId { get; set; }
        public int DepartmantId { get; set; }
        public string UploadProfilePicture { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        public string Address { get; set; }
        public string Level { get; set; }
        public bool Tick { get; set; }
        public List<int> UserSkills { get; set; }
        public Update()
        {
            this.UserSkills = new List<int>();
        }
     }
}