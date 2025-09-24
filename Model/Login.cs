using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
namespace AppTaller.Model
{
    internal class Login
    {

        private Conexion conexion = new Conexion();

        public bool ValidarUsuario(int id, string contrasena)
        {
            using (SqlConnection conn = conexion.AbrirConexion())
            {
                string query = "SELECT COUNT(*) FROM Usuario WHERE id = @usuario AND contrasena = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", id);
                    cmd.Parameters.AddWithValue("@password", contrasena);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
