using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Settings.Web.Models
{
    public class EntryVM
    {
        [Display(Name = "Entry Key")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,99}$", ErrorMessage = "{0} must be less than 100 letters, numbers or underscores, and must begin with a letter")]
        public string Key { get; set; }

        [Display(Name = "Entry Value")]
        [StringLength(1000, ErrorMessage = "{0} must be less than {1} characters")]
        public string Value { get; set; }
    }
}