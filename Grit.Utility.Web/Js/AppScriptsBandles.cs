using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Grit.Utility.Web.JS
{
    public static class AppScriptsBandles
    {
        public class Item
        {
            public string Key { get; set; }
            public string Bundle { get; set; }
            public string Include { get; set; }
        }

        static AppScriptsBandles()
        {
            Items = new List<Item>();
        }

        public static IEnumerable<Item> Items { get; private set; }

        public static void AddFolder(string scriptsPath, BundleCollection bundles)
        {
            Items = GetScriptsInFolder("~/Scripts/app/");
            foreach (var item in Items)
            {
                bundles.Add(new ScriptBundle(item.Bundle).Include(item.Include));
            }
        }

        private static IEnumerable<Item> GetScriptsInFolder(string scriptsPath)
        {
            if (!scriptsPath.StartsWith("~"))
            {
                throw new ArgumentException("Scripts path MUST start with ~");
            }
            IList<Item> list = new List<Item>();
            scriptsPath = scriptsPath.TrimEnd('/') + "/";
            string scriptsAbstractPath = HttpContext.Current.Server.MapPath(scriptsPath);
            foreach (string path in Directory.GetFiles(scriptsAbstractPath, "*.*", SearchOption.AllDirectories))
            {
                string relativePath = path.Substring(scriptsAbstractPath.Length).Replace('\\', '/');
                string slug = relativePath.Replace('.', '-').Replace('/', '-');
                Item item = new Item
                {
                    Key = slug,
                    Bundle = Path.Combine("~/bundles/", slug),
                    Include = Path.Combine(scriptsPath, relativePath)
                };
                list.Add(item);
            }
            return list;
        }

        public static IHtmlString GetRequireJsPathScripts()
        {
            if (!Items.Any()) return null;
            StringBuilder sb = new StringBuilder();
            foreach(var item in Items) 
            {
                sb.AppendFormat("'{0}': '{1}',", item.Key, Scripts.Url(item.Bundle));
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return new System.Web.Mvc.MvcHtmlString(sb.ToString());
        }

        public static IHtmlString GetRequireJsPathScripts(IEnumerable<string> keys, bool commaPrefix = true, bool commaPostfix = false)
        {
            if (!Items.Any()) return null;
            if (keys == null || !keys.Any()) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var item in Items)
            {
                if (keys != null && keys.Any(x=>string.Compare(item.Key,x,true) == 0))
                {
                    sb.AppendFormat("'{0}': '{1}',", item.Key, Scripts.Url(item.Bundle));
                }
            }
            if (sb.Length > 0)
            {
                if (!commaPostfix)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                if(commaPrefix)
                {
                    sb.Insert(0, ", ");
                }
            }
            return new MvcHtmlString(sb.ToString());
        }

        public static IHtmlString GetRequireJsDeps(IEnumerable<string> keys, IEnumerable<string> appendKeys = null, bool commaPrefix = true, bool commaPostfix = false)
        {
            if ((keys == null || !keys.Any()) && (appendKeys == null || !appendKeys.Any())) return null;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}deps: [", commaPrefix ? ", " : string.Empty);
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    sb.AppendFormat("'{0}', ", key);
                }
            }
            if (appendKeys != null)
            {
                foreach (var key in appendKeys)
                {
                    sb.AppendFormat("'{0}', ", key);
                }
            }
            sb.Remove(sb.Length - 2, 2);
            sb.AppendFormat("]{0}", commaPostfix ? ", " : string.Empty);
            return new MvcHtmlString(sb.ToString());
        }
    }
}
