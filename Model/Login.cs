using System;
using System.Data;
using System.Data.SqlClient;

namespace AppTaller.Model
{
    internal class Login
    {
        private Conexion conexion = new Conexion();

        public Usuario ValidarUsuario(int id, string contrasena)
        {
            using (SqlConnection conn = conexion.AbrirConexion())
            using (SqlCommand cmd = new SqlCommand("sp_Login", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", id);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            id = (int)reader["id"],
                            nombre = reader["nombre"].ToString(),
                            correo = reader["correo"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            estatus = (bool)reader["estatus"],
                            idRol = (int)reader["idRol"],
                        };
                    }
                }
            }
            return null;
        }
    }
}
