using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Settings.Web.Models
{
    public class SecurityKeyVM
    {
        [Display(Name = "AES Key (Base64)")]
        public string Key { get; set; }

        [Display(Name = "AES IV (Base64)")]
        public string IV { get; set; }

        [Display(Name = "RSA Public Key (XML)")]
        public string PublicKey { get; set; }

        [Display(Name = "RSA Private Key (XML)")]
        public string PrivateKey { get; set; }
    }
}