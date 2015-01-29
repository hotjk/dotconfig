using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Grit.Utility.Web.Json
{
    public class JsonNetModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null; // not JSON request
            }

            var request = controllerContext.HttpContext.Request;
            request.InputStream.Position = 0;
            var incomingData = new StreamReader(request.InputStream).ReadToEnd();

            if (String.IsNullOrEmpty(incomingData))
            {
                return null; // not JSON request
            }
            object ret = JsonConvert.DeserializeObject(incomingData, bindingContext.ModelType);
            return ret;
        }
    }
}
