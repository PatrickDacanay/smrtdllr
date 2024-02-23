using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDollarWorker.Access.DbApps
{
    public class DbBase
    {
        readonly string _connectionString;
        public DbBase(string connectionstring)
        {
            _connectionString = connectionstring;
        }
        protected MySqlConnection GetConnections()
        {
            return new MySqlConnection()
            {
                ConnectionString = _connectionString
            };
        }
    }
}
