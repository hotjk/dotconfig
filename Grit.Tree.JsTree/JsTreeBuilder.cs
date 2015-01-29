using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Tree.JsTree
{
    public class JsTreeBuilder<T>
    {
        private Func<T, string> GetText { get; set; }
        private Func<T, int> GetContent { get; set; }
        private Func<T, IEnumerable<int>> GetElements { get; set; }
        public JsTreeBuilder(
            Func<T, string> getText,
            Func<T, int> getContent,
            Func<T, IEnumerable<int>> getElements = null)
        {
            this.GetText = getText;
            this.GetContent = getContent;
            this.GetElements = getElements;
        }

        public JsTreeNode Build(Node node, IEnumerable<T> entities)
        {
            JsTreeNode root = BuildNode(node, entities);

            ISet<int> set = new HashSet<int>();
            node.Summarize(ref set);
            var unused = entities.Where(n => !set.Contains(GetContent(n)));
            foreach(var entity in unused)
            {
                root.AddChild(new JsTreeNode
                {
                    text = GetText(entity),
                    data = new JsTreeNodeData { content = GetContent(entity),
                                                elements = GetElements == null ? null : GetElements(entity) },
                    state = new JsTreeNodeState { opened = true }
                });
            }

            return root;
        }

        private JsTreeNode BuildNode(Node node, IEnumerable<T> entities)
        {
            JsTreeNode jsTreeNode = null;
            if (node.Parent == null) // root node is a dummy node.
            {
                jsTreeNode = new JsTreeNode();
            }
            else
            {
                var entity = entities.SingleOrDefault(n => GetContent(n) == node.Data);
                if (entity != null) // if entity has been deleted, tree node is also obsoleted.
                {
                    jsTreeNode = new JsTreeNode
                    {
                        text = GetText(entity),
                        data = new JsTreeNodeData { content = GetContent(entity),
                                                    elements = GetElements == null ? null : GetElements(entity) },
                        state = new JsTreeNodeState { opened = true }
                    };
                }
            }

            if (jsTreeNode != null && node.Children != null && node.Children.Count > 0)
            {
                foreach (Node child in node.Children)
                {
                    var childJsTreeNode = BuildNode(child, entities);
                    if(childJsTreeNode != null)
                    {
                        jsTreeNode.AddChild(childJsTreeNode);
                    }
                }
            }

            return jsTreeNode;
        }
    }
}
