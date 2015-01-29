using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public class Envelope
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public string Data { get; set; }
    }
}
