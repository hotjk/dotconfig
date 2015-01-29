using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Model
{
    public interface INodeRepository
    {
        IEnumerable<Node> GetNodes();
        Node GetNode(int nodeId);
        IEnumerable<Node> GetNodes(int[] nodes);
        bool SaveNode(Node node);
        bool DeleteNode(Node node);
    }
}
