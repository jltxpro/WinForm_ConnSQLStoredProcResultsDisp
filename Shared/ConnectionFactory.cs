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
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }


    /*
        public class ConnectionFactory  
        {
            private CreateConnectionSQL connSQL = new CreateConnectionSQL();

            //public SqlConnection CreateConnection(string connString)
            public MySqlConnection CreateConnection(string connString)
            {
                return connSQL.CreateConnection(connString);
            }

        }

        public class CreateConnectionSQL
        {
            //public SqlConnection CreateConnection(string connString)
            public MySqlConnection CreateConnection(string connString)
            {
                //SqlConnection conn = new SqlConnection(connString);
                MySqlConnection conn = new MySqlConnection(connString);
                return conn;

            }
        }

     */


}
