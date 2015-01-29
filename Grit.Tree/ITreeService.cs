using System;
using System.Collections.Generic;

namespace Grit.Tree
{
    public interface ITreeService
    {
        Node GetTree(int treeId);
        IEnumerable<Node> GetTreeNodes(int tree);
        void SaveTree(Node root);
    }
}
