using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Tree.JsTree
{
    public class JsTreeNodeData
    {
        public int content { get; set; }
        public IEnumerable<int> elements { get; set; }
    }
}
