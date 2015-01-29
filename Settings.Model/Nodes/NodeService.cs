using Grit.Utility.Security;
using Settings.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Model
{
    public class NodeService : INodeService
    {
        private static readonly byte[] KEY = Convert.FromBase64String(ConfigurationManager.AppSettings["PersistenceKey"]);
        private static readonly byte[] IV = Convert.FromBase64String(ConfigurationManager.AppSettings["PersistenceIV"]);

        public NodeService(INodeRepository nodeRepository)
        {
            this.NodeRepository = nodeRepository;
        }
        private INodeRepository NodeRepository { get; set; }

        public bool SaveNode(Node node)
        {
            RijndaelManager rsa = new RijndaelManager(KEY, IV);
            node.Entries.ForEach(x => {
                x.NodeId = node.NodeId;
                x.Value = rsa.Encrypt(x.Value);
            });
            return NodeRepository.SaveNode(node);
        }

        public bool DeleteNode(Node node)
        {
            return NodeRepository.DeleteNode(node);
        }

        public Node GetNode(int nodeId)
        {
            var node = NodeRepository.GetNode(nodeId);
            RijndaelManager rsa = new RijndaelManager(KEY, IV);
            node.Entries.ForEach(x => {
                x.Value = rsa.Decrypt(x.Value);
            });
            return node;
        }

        public IEnumerable<Node> GetNodes()
        {
            return NodeRepository.GetNodes();
        }

        public IEnumerable<Node> GetNodes(int[] nodes)
        {
            return NodeRepository.GetNodes(nodes);
        }

        public SettingsResponse GetClientSettings(Client client, Grit.Tree.Node tree)
        {
            var clientNodes = GetNodes(client.Nodes);
            var allNodes = GetNodes();

            SettingsResponse resp = new SettingsResponse(client.Name);
            var path = new List<Grit.Tree.Node>(5);
            foreach (var node in clientNodes)
            {
                if (node.Entries == null || !node.Entries.Any()) continue;
                path.Clear();
                tree.FindByData(node.NodeId, path);

                string strPath = string.Join("/",
                    path.Select(n => allNodes.FirstOrDefault(x => x.NodeId == n.Data)).Select(n => n.Name).Reverse())
                    + "/";

                resp.Entries.AddRange(node.Entries.Select(n => new SettingsResponse.Entry { Path = strPath + n.Key, Value = n.Value }));
            }
            return resp;
        }
    }
}
