using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Settings.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string client = ConfigurationManager.AppSettings["SettingsClient"];
            string api = ConfigurationManager.AppSettings["SettingsAPI"];
            string pattern = ConfigurationManager.AppSettings["SettingsPattern"];
            string key = ConfigurationManager.AppSettings["SettingsPrivateKey"];
            SettingsProxy service = new SettingsProxy();
            var settings = await service.GetSettings(client, api, pattern, key);
            return View(settings);
            //return View(settings.Filter("Product"));
        }
    }
}