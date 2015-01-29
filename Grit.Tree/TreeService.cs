using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Grit.Tree
{
    public class TreeService : ITreeService
    {
        public TreeService(ITreeRepository treeRepository, string table)
        {
            this.TreeRepository = treeRepository;
            this.TreeRepository.Table = table;
        }
        private ITreeRepository TreeRepository { get; set; }

        public Node GetTree(int tree)
        {
            var nodes = TreeRepository.GetTreeNodes(tree);
            Node root = nodes.FirstOrDefault();
            if(root == null)
            {
                root = new Node(tree);
            }
            foreach(var node in nodes.Skip(1))
            {
                root.AddChild(node);
            }
            return root;
        }

        public IEnumerable<Node> GetTreeNodes(int tree)
        {
            return TreeRepository.GetTreeNodes(tree);
        }

        public void SaveTree(Node root)
        {
            IList<Node> nodes = new List<Node>();
            root.Each(x=>nodes.Add(x));
            TreeRepository.SaveTreeNodes(nodes);
        }
    }
}
