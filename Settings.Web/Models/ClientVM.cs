using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grit.Utility.Web.Extensions;
using Settings.Model;
using AutoMapper;

namespace Settings.Web.Models
{
    public class ClientVM
    {
        public int ClientId { get; set; }

        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]{0,99}$", ErrorMessage = "{0} must be less than 100 letters, numbers or underscores, and must begin with a letter")]
        public string Name { get; set; }

        [Display(Name = "RSA Public Key")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(2000, ErrorMessage = "{0} must be less than {1} characters")]
        public string PublicKey {get;set;}

        public bool Deleted { get; set; }

        public int Version { get; set; }

        static ClientVM()
        {
            Mapper.CreateMap<Settings.Model.Client, ClientVM>();
            Mapper.CreateMap<ClientVM, Settings.Model.Client>();
        }

        public static Settings.Model.Client ToModel(ClientVM vm)
        {
            return Mapper.Map<Settings.Model.Client>(vm);
        }

        public static ClientVM FromModel(Settings.Model.Client m = null)
        {
            ClientVM vm;
            if (m == null)
            {
                vm = new ClientVM();
            }
            else
            {
                vm = Mapper.Map<ClientVM>(m);
            }
            return vm;
        }
    }
}