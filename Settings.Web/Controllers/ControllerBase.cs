using BrockAllen.CookieTempData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Settings.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected override ITempDataProvider CreateTempDataProvider()
        {
            return new CookieTempDataProvider();
        }

        public string Info
        {
            get
            {
                return TempData["Info"] as string;
            }
            set
            {
                TempData["Info"] = value;
            }
        }
    }
}