using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Tree.JsTree
{
    public class JsTreeParser
    {
        public Node Parse(int tree, IList<JsTreeNode> nodes)
        {
            JsTreeNode root = new JsTreeNode { children = nodes, data = new JsTreeNodeData() };
            return Parse(tree, root);
        }

        public Node Parse(int tree, JsTreeNode jsTreeNode)
        {
            int id = 0;
            Node root = null;
            ParseNode(tree, jsTreeNode, ref root, ref id);
            return root;
        }

        private void ParseNode(int tree, JsTreeNode jsTreeNode, ref Node parent, ref int id)
        {
            Node node = null;
            if (parent == null)
            {
                node = new Node(tree, id++);
                parent = node;
            }
            else
            {
                node = new Node(tree, id++, parent.Id, jsTreeNode.data.content, jsTreeNode.data.elements);
                parent.AddChild(node);
            }

            if (jsTreeNode.children != null && jsTreeNode.children.Count > 0)
            {
                foreach (var child in jsTreeNode.children)
                {
                    ParseNode(tree, child, ref node, ref id);
                }
            }
        }
    }
}
