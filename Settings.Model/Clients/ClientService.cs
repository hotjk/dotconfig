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
    public class ClientService : IClientService
    {
        public ClientService(IClientRepository clientRepository)
        {
            this.ClientRepository = clientRepository;
        }
        private IClientRepository ClientRepository { get; set; }

        public Client GetClient(int clientId)
        {
            return ClientRepository.GetClient(clientId);
        }

        public Client GetClient(string name)
        {
            return ClientRepository.GetClient(name);
        }

        public IEnumerable<Client> GetClients()
        {
            return ClientRepository.GetClients();
        }

        public bool SaveClient(Client client)
        {
            return ClientRepository.SaveClient(client);
        }

        public bool SaveClientNodes(IEnumerable<Client> clients)
        {
            return ClientRepository.SaveClientNodes(clients);
        }

        public bool DeleteClient(Client client)
        {
            return ClientRepository.DeleteClient(client);
        }
    }
}
