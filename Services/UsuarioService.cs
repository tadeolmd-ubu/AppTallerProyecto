using AppTaller.EF;
using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class UsuarioService
    {


        // CREATE
        public void CrearUsuario(Usuario usuario)
        {
            using (var context = new efAppDbContext())
            {
                context.Usuario.Add(usuario);
                context.SaveChanges();
            }
        }

        // READ (todos)
        public List<Usuario> ObtenerUsuarios()
        {
            using (var context = new efAppDbContext())
            {
                return context.Usuario.ToList();
            }
        }

        // READ (por id)
        public Usuario ObtenerPorId(int id)
        {
            using (var context = new efAppDbContext())
            {
                return context.Usuario.FirstOrDefault(u => u.id == id);
            }
        }

        // UPDATE
        public void ActualizarUsuario(Usuario usuario)
        {
            using (var context = new efAppDbContext())
            {
                context.Usuario.Update(usuario);
                context.SaveChanges();
            }
        }

        // DELETE
        public void EliminarUsuario(int id)
        {
            using (var context = new efAppDbContext())
            {
                var usuario = context.Usuario.FirstOrDefault(u => u.id == id);
                if (usuario != null)
                {
                    context.Usuario.Remove(usuario);
                    context.SaveChanges();
                }
            }
        }

    }
}
