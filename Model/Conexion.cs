using System.Configuration;
using System.Data.SqlClient;

namespace AppTaller.Model
{
    internal class Conexion
    {
        public SqlConnection AbrirConexion()
        {
            var connString = ConfigurationManager.ConnectionStrings["DbTaller"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            return conn;
        }
    }
}
