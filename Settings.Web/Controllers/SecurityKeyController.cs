using Grit.Sequence;
using Grit.Tree.JsTree;
using Grit.Utility.Security;
using Grit.Utility.Web.Json;
using Settings.Model;
using Settings.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Settings.Web.Controllers
{
    public class SecurityKeyController : ControllerBase
    {
        [HttpGet]
        [Auth]
        public ActionResult Index()
        {
            string key, iv;
            RijndaelManager.GenerateKeyAndIV(out key, out iv);
            string publicKey, privateKey;
            RSAManager.GenerateKeyAndIV(out publicKey, out privateKey);

            return View(new SecurityKeyVM { 
                Key = key,
                IV = iv,
                PublicKey = publicKey, 
                PrivateKey = privateKey 
            });
        }
   }
}