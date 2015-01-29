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
    public class NodeRepository : BaseRepository, INodeRepository
    {
        public NodeRepository(SqlOption option) : base(option) { }

        public IEnumerable<Node> GetNodes()
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Node>(
@"SELECT `NodeId`, `Name`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_node` 
WHERE Deleted = 0;");
            }
        }

        public IEnumerable<Node> GetNodes(int[] ids)
        {
            if (ids == null || !ids.Any())
            {
                return new List<Node>();
            }
            using (IDbConnection connection = OpenConnection())
            {
                using (var multi = connection.QueryMultiple(
@"SELECT `NodeId`, `Name`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_node` WHERE `NodeId` IN @Ids AND Deleted = 0;
SELECT `NodeId`, `Key`, `Value` FROM `Settings_Entry` WHERE `NodeId` IN @Ids;",
                    new { Ids = ids }))
                {
                    var nodes = multi.Read<Node>();
                    var entries = multi.Read<Entry>();
                    foreach (var node in nodes)
                    {
                        node.Entries = entries.Where(n => n.NodeId == node.NodeId).ToList();
                    }
                    return nodes;
                }
            }
        }

        public Node GetNode(int nodeId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (var multi = connection.QueryMultiple(
@"SELECT `NodeId`, `Name`, `Version`, `CreateAt`, `UpdateAt` FROM `settings_node` 
WHERE `NodeId` = @NodeId AND Deleted = 0;
SELECT `NodeId`, `Key`, `Value` FROM `Settings_Entry` WHERE `NodeId` = @NodeId;", 
                    new { NodeId = nodeId })) {
                    var node = multi.Read<Node>().SingleOrDefault();
                    if(node == null) return null;
                    node.Entries = multi.Read<Entry>().ToList();
                    return node;
                }
            }
        }

        public bool SaveNode(Node node)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    Node found = connection.Query<Node>(
@"SELECT `NodeId`, `Version` FROM `settings_node` 
WHERE `NodeId` = @NodeId FOR UPDATE;",
                        new { NodeId = node.NodeId }).SingleOrDefault();
                    if (found == null)
                    {
                        if (1 != connection.Execute(
@"INSERT INTO `settings_node` 
(`NodeId`, `Name`, `Version`, `CreateAt`, `UpdateAt`)
VALUES (@NodeId, @Name, @Version, @CreateAt, @UpdateAt);", node))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (found.Version != node.Version)
                        {
                            return false;
                        }
                        node.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_node` 
SET `Name` = @Name, `Version` = @Version, `UpdateAt` = @UpdateAt 
WHERE NodeId = @NodeId;", node);
                    }
                    connection.Execute(
@"DELETE FROM `settings_entry` WHERE `NodeId` = @NodeId;", new { NodeId = node.NodeId });
                    connection.Execute(
@"INSERT INTO `settings_entry` (`NodeId`, `Key`, `Value`) VALUES (@NodeId, @Key, @Value);",
                        node.Entries);
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool DeleteNode(Node node)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    Node found = connection.Query<Node>(
@"SELECT `NodeId`, `Version` FROM `settings_node` 
WHERE `NodeId` = @NodeId FOR UPDATE;",
                        new { NodeId = node.NodeId }).SingleOrDefault();
                    if (found == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (found.Version != node.Version)
                        {
                            return false;
                        }
                        node.Version++;
                        int n = connection.Execute(
@"UPDATE `settings_node` 
SET `Deleted` = 1, `DeleteAt` = @DeleteAt 
WHERE NodeId = @NodeId;", node);
                        transaction.Commit();
                        return 1 == n;
                    }
                }
            }
        }
    }
}
