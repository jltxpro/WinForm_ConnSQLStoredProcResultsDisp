using System.Data.SqlClient;

namespace Utilities
{
    public class ConnectionFactory  
    {
        private CreateConnectionSQL connSQL = new CreateConnectionSQL();

        public SqlConnection CreateConnection(string connString)
        {
            return connSQL.CreateConnection(connString);
        }

    }

    public class CreateConnectionSQL
    {
        public SqlConnection CreateConnection(string connString)
        {
            SqlConnection conn = new SqlConnection(connString);
            return conn;

        }
    }
}
