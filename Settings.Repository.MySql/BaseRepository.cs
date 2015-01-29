using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Repository.MySql
{
    public class BaseRepository
    {
        public BaseRepository(SqlOption option)
        {
            this.ConnectionString = option.ConnectionString;
        }
        private string ConnectionString { get; set; }

        public IDbConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
