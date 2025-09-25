using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Model
{
    internal class Conexion
    {
        static public string dbn = "dbTaller";
        static public string server = @"E4";
        static public string seguridad = "Integrated Security=True";

        static public string miConexion = "Data Source=" + server + ";Initial Catalog=" + dbn + ";" + seguridad + ";Persist Security Info=True;";

        public SqlConnection AbrirConexion()
        {
            SqlConnection conn = new SqlConnection(miConexion);
            conn.Open();
            return conn;
        }

    }
}
