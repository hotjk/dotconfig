using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Tree
{
    public interface ITreeRepository
    {
        string Table { get; set; }
        IEnumerable<Node> GetTreeNodes(int treeId);
        void SaveTreeNodes(IList<Node> nodes);
    }
}
