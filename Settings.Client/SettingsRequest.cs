using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Client
{
    public class SettingsRequest
    {
        public SettingsRequest(string client, string pattern)
        {
            this.Client = client;
            this.Pattern = pattern;
        }
        public string Client { get; private set; }
        public string Pattern { get; private set; }
    }
}
