using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Grit.Tree.Repository.MySql
{
    public class TreeRepository : BaseRepository, ITreeRepository
    {
        public TreeRepository(SqlOption option) : base(option) { }

        // CREATE TABLE `tree` ( `Tree` int(11) NOT NULL, `Id` int(11) NOT NULL, `Parent` int(11) DEFAULT NULL, `Data` int(11) DEFAULT NULL, PRIMARY KEY (`Id`,`Tree`) ) ENGINE=InnoDB DEFAULT CHARSET=utf8;
        
        public string Table { get; set; }
        
        public IEnumerable<Node> GetTreeNodes(int tree)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Node>(
                    string.Format("SELECT `Tree`, `Id`, `Parent`, `Data` FROM `{0}` WHERE `Tree` = @Tree ORDER BY `Parent`, `Id`;",
                    Table), new { Tree = tree });
            }
        }

        public void SaveTreeNodes(IList<Node> nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }

            using (IDbConnection connection = OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(string.Format("DELETE FROM `{0}` WHERE `Tree` = @Tree;",
                        Table), new { Tree = nodes.First().Tree });
                    connection.Execute(string.Format("INSERT INTO `{0}` (`Tree`, `Id`, `Parent`, `Data`) VALUES (@Tree, @Id, @Parent, @Data);",
                        Table), nodes);
                    transaction.Commit();
                }
            }
        }
    }
}
