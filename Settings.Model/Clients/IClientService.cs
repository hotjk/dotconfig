using Settings.Client;
using System;
using System.Collections.Generic;

namespace Settings.Model
{
    public interface IClientService
    {
        Client GetClient(int clientId);
        Client GetClient(string name);
        IEnumerable<Client> GetClients();
        bool SaveClient(Client client);
        bool SaveClientNodes(IEnumerable<Client> clients);
        bool DeleteClient(Client client);
    }
}
