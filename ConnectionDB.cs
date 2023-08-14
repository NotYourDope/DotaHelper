using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace DotaHelper
{
    class ConnectionDB
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-4R5RIHS\SQLSERVER;Initial Catalog=DotaHelper;Integrated Security=True");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed) { sqlConnection.Open(); }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open) { sqlConnection.Close(); }
        }

        public SqlConnection GetConnection() { return sqlConnection; }
    }
}
