using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace Utilities
{

    public class ConnectionFactory
    {
        private CreateConnectionSQL conn = new CreateConnectionSQL();

        public IDbConnection CreateConnection(string connString)
        {
            return conn.CreateConnection(connString);
        }

    }

    public class CreateConnectionSQL
    {
        public IDbConnection CreateConnection(string connString)
        {
            //MySqlConnection conn = new MySqlConnection(connString);
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
    }


}
