using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MLMModel
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required, EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; }

        public int? SponsorId { get; set; }
    }

}
