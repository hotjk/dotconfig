using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Tree
{
    public class Node
    {
        public Node(int tree, int id = 0)
            : this(tree, id, null, null, null)
        {
        }

        public Node(int tree, int id, int? parent, int? data)
            : this(tree, id, parent, data, null)
        {
        }

        public Node(int tree, int id, int? parent, int? data, IEnumerable<int> elements)
        {
            this.Tree = tree;
            this.Id = id;
            this.Parent = parent;
            this.Data = data;
            this.Elements = elements;
        }

        public int Tree { get; private set; }
        public int Id { get; private set; }
        public int? Parent { get; private set; }
        public IList<Node> Children { get; private set; }
        public int? Data { get; private set; }
        public IEnumerable<int> Elements { get; private set; }

        public bool AddChild(Node node)
        {
            if (node.Parent == this.Id)
            {
                if (Children == null)
                {
                    Children = new List<Node>();
                }
                Children.Add(node);
                return true;
            }
            else
            {
                if (Children != null)
                {
                    foreach (Node child in Children)
                    {
                        if (child.AddChild(node))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Node FindByData(int data, List<Node> path = null)
        {
            if (this.Data == data)
            {
                return this;
            }
            if (Children != null)
            {
                foreach (Node child in Children)
                {
                    var found = child.FindByData(data, path);
                    if (found != null)
                    {
                        if (path != null)
                        {
                            path.Add(child);
                        }
                        return found;
                    }
                }
            }
            return null;
        }

        public void Summarize(ref ISet<int> set)
        {
            if (Data.HasValue)
            {
                set.Add(Data.Value);
            }
            if (Children != null && Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    child.Summarize(ref set);
                }
            }
        }

        public void Flat(ref IList<Node> nodes)
        {
            nodes.Add(this);
            if(Children != null && Children.Count > 0)
            {
                foreach(var child in Children)
                {
                    child.Flat(ref nodes);
                }
            }
        }

        public void Each(Action<Node> action)
        {
            action(this);
            if (Children != null && Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    child.Each(action);
                }
            }
        }
    }
}
