using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Grit.Utility.Web.Json
{
    public static class JsonExtensions
    {
        public static MvcHtmlString Jsonize(this HtmlHelper helper, string variableName, object obj, bool scriptContainer = true, bool ignoreNullValue = false)
        {
            StringBuilder str = new StringBuilder();
            if (scriptContainer)
            {
                str.Append("<script type='text/javascript'>\n");
            }
            str.Append("var ");
            str.Append(variableName);
            str.Append(" = ");

            if (obj == null)
            {
                str.Append("null");
            }
            else
            {
                if (ignoreNullValue)
                {
                    str.Append(JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                }
                else
                {
                    str.Append(JsonConvert.SerializeObject(obj));
                }
            }

            str.Append(";");
            if (scriptContainer)
            {
                str.Append("\n</script>");
            }
            return MvcHtmlString.Create(str.ToString());
        }
        public static MvcHtmlString Json(this HtmlHelper helper, object obj, bool ignoreNullValue = false)
        {
            StringBuilder str = new StringBuilder();
            if (obj == null)
            {
                str.Append("null");
            }
            else
            {
                if (ignoreNullValue)
                {
                    str.Append(JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                }
                else
                {
                    str.Append(JsonConvert.SerializeObject(obj, new JavaScriptDateTimeConverter()));
                }
            }
            return MvcHtmlString.Create(str.ToString());
        }
    }
}
