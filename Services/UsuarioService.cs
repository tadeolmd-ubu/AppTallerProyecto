using AppTaller.EF;
using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class UsuarioService
    {
        private readonly EF.efAppDbContext _context;
        public UsuarioService(EF.efAppDbContext context) {
            _context = context;
        }
        public void CrearUsuario(Usuario usuario) {
            if (usuario.id == 0)
                usuario.id = _context.SiguienteId("Usuario");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Usuario @opcion = 1, @id = {0}, @nombre = {1}, @contrasena = {2}, @correo = {3}, @telefono = {4}, @estatus = {5}, @idRol = {6}",
                usuario.id, usuario.nombre, usuario.contrasena, usuario.correo, usuario.telefono, usuario.estatus, usuario.idRol);
        }
        public void ActualizarUsuario(Usuario usuario) {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Usuario @opcion = 2, @id = {0}, @nombre = {1}, @contrasena = {2}, @correo = {3}, @telefono = {4}, @estatus = {5}, @idRol = {6}",
                usuario.id, usuario.nombre, usuario.contrasena, usuario.correo, usuario.telefono, usuario.estatus, usuario.idRol);
        }
        public void CrearOActualizarUsuario(Usuario usuario) {
            var existe = _context.Usuario.Find(usuario.id);
            if (existe == null)
                CrearUsuario(usuario);
            else
                ActualizarUsuario(usuario);
        }
        public List<Usuario> ObtenerUsuarios() {
            return _context.Usuario.FromSqlRaw("EXEC sp_Usuario @opcion = 4").ToList();
        }
        public Usuario ObtenerPorId(int id) {
            return _context.Usuario.FromSqlRaw("EXEC sp_Usuario @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }
        public void EliminarUsuario(int id) {
            _context.Database.ExecuteSqlRaw("EXEC sp_Usuario @opcion = 3, @id = {0}", id);
        }
        public int ObtenerSigienteIdUsuario(){
            return _context.SiguienteId("Usuario");
        }
    }
}
