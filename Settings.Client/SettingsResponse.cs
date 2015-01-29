using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Client
{
    public class SettingsResponse
    {
        public class Entry
        {
            public string Path { get; set; }
            public string Value { get; set; }
        }

        public SettingsResponse(string client)
        {
            this.Client = client;
            this.Entries = new List<Entry>(10);
        }
        public string Client { get; private set; }
        public List<Entry> Entries { get; private set; }

        public SettingsResponse Filter(string pattern)
        {
            pattern = pattern.Trim(new char[] { '/' });
            SettingsResponse resp = new SettingsResponse(this.Client);
            resp.Entries = this.Entries.Where(n => Match(n.Path, pattern)).ToList();
            return resp;
        }

        public T Get<T>(string pattern, T defaultValue = default(T))
        {
            var found = this.Entries.FirstOrDefault(n => Match(n.Path, pattern));
            if (found == null)
            {
                return defaultValue;
            }
            try
            {
                T value = (T)Convert.ChangeType(found.Value, typeof(T));
                return value;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool Match(string path, string pattern)
        {
            path = path.Trim(new char[] { '/' });
            
            if (string.IsNullOrWhiteSpace(pattern) || pattern == "*") return true;
            int idxPath = 0, idxPattern = 0;
            for (; idxPattern < pattern.Length; idxPattern++)
            {
                char cPattern = pattern[idxPattern];
                if (cPattern == '*')
                {
                    idxPath = path.IndexOf('/', idxPath);
                    if (idxPath == -1)
                    {
                        return idxPattern + 1 == pattern.Length;
                    }
                }
                else
                {
                    if (cPattern != path[idxPath++])
                    {
                        return false;
                    }
                }
            }
            return (idxPath >= path.Length || path[idxPath] == '/');
        }
    }
}
