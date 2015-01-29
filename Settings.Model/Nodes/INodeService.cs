using Settings.Client;
using System;
using System.Collections.Generic;

namespace Settings.Model
{
    public interface INodeService
    {
        IEnumerable<Node> GetNodes();
        Node GetNode(int nodeId);
        IEnumerable<Node> GetNodes(int[] nodes);
        bool SaveNode(Node node);
        bool DeleteNode(Node node);
        SettingsResponse GetClientSettings(Client client, Grit.Tree.Node tree);
    }
}
