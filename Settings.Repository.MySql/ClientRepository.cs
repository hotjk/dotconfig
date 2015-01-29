using Settings.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Settings.Repository.MySql
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(SqlOption option) : base(option) { }

        public Client GetClient(int clientId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (var multi = connection.QueryMultiple(
@"SELECT `ClientId`, `Name`, `PublicKey`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_client` WHERE `ClientId` = @ClientId AND `Deleted` = 0;
SELECT `NodeId` FROM `settings_client_node` WHERE `ClientId` = @ClientId;", 
                    new {  ClientId = clientId }))
                {
                    var client = multi.Read<Client>().SingleOrDefault();
                    if (client != null)
                    {
                        client.Nodes = multi.Read<int>().ToArray();
                    }
                    return client;
                }
            }
        }

        public Client GetClient(string name)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var client = connection.Query<Client>(
@"SELECT `ClientId`, `Name`, `PublicKey`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_client` WHERE `Name` = @Name AND `Deleted` = 0;", 
                    new { Name = name }).SingleOrDefault();
                if(client == null)
                {
                    return null;
                }
                client.Nodes = connection.Query<int>(
@"SELECT `NodeId` FROM `settings_client_node` WHERE `ClientId` = @ClientId;", new { ClientId = client.ClientId }).ToArray();
                return client;
            }
        }

        private class ClientNode
        {
            public int ClientId { get; set; }
            public int NodeId { get; set; }
        }

        public IEnumerable<Client> GetClients()
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (var multi = connection.QueryMultiple(
@"SELECT `ClientId`, `Name`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_client` WHERE `Deleted` = 0;
SELECT `ClientId`, `NodeId` FROM `settings_client_node`;"))
                {
                    var clients = multi.Read<Client>();
                    var nodes = multi.Read<ClientNode>(); 
                    foreach(var client in clients)
                    {
                        client.Nodes = nodes.Where(n => n.ClientId == client.ClientId).Select(n => n.NodeId).ToArray();
                    }
                    return clients;
                }
            }
        }

        public bool SaveClient(Client client)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    Client found = connection.Query<Client>(
@"SELECT `ClientId`, `Version` FROM `settings_client` WHERE `ClientId` = @ClientId FOR UPDATE;",
                        new { ClientId = client.ClientId }).SingleOrDefault();
                    if (found == null)
                    {
                        if (1 != connection.Execute(
@"INSERT INTO `settings_client` 
(`ClientId`, `Name`, `PublicKey`, `Version`, `CreateAt`, `UpdateAt`)
VALUES (@ClientId, @Name, @PublicKey, @Version, @CreateAt, @UpdateAt);", client))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (found.Version != client.Version)
                        {
                            return false;
                        }
                        client.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_client` 
SET `Name` = @Name, `PublicKey` = @PublicKey, `Version` = @Version, `UpdateAt` = @UpdateAt 
WHERE ClientId = @ClientId;", client);
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool DeleteClient(Client client)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    Client found = connection.Query<Client>(
@"SELECT `ClientId`, `Version` FROM `settings_client` WHERE `ClientId` = @ClientId FOR UPDATE;",
                        new { ClientId = client.ClientId }).SingleOrDefault();
                    if (found == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (found.Version != client.Version)
                        {
                            return false;
                        }
                        client.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_client` 
SET `Deleted` = 1, `DeleteAt` = @DeleteAt 
WHERE ClientId = @ClientId;", client);
                        transaction.Commit();
                        return 1 == n;
                    }
                }
            }
        }

        public bool SaveClientNodes(IEnumerable<Client> clients)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    foreach (var client in clients)
                    {
                        connection.Execute(
@"DELETE FROM `settings_client_node` WHERE `ClientId` = @ClientId;", client);
                        connection.Execute(
@"INSERT INTO `settings_client_node` (ClientId, NodeId) VALUES (@ClientId, @NodeId);",
                            client.Nodes.Select(n => new { ClientId = client.ClientId, NodeId = n }));
                    }
                    transaction.Commit();
                }
            }
            return true;
        }
    }
}
