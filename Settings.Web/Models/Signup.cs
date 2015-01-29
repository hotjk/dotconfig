using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Settings.Web.Models
{
    public class SignupVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,49}$", ErrorMessage = "{0} must be less than 50 letters, numbers or underscores, and must begin with a letter")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,49}$", ErrorMessage = "{0} must be less than 50 letters, numbers or underscores, and must begin with a letter")]
        public string Password { get; set; }

        [Display(Name = "Re-enter password")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,49}$", ErrorMessage = "{0} must be less than 50 letters, numbers or underscores, and must begin with a letter")]
        [Compare("Password", ErrorMessage="Password do not match")]
        public string PasswordRe { get; set; }
    }
}