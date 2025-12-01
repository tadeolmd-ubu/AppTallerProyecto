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
        private readonly EF.efAppDbContext _context;
        public UsuarioService(EF.efAppDbContext context) {
            _context = context;
        }
        // CREATE
        public void CrearUsuario(Usuario usuario) {            
                _context.Usuario.Add(usuario);
                _context.SaveChanges();            
        }
        public void ActualizarUsuario(Usuario usuario) {
            var existente = _context.Usuario.Find(usuario.id);
            if (existente == null) 
                return;

            _context.Entry(existente).CurrentValues.SetValues(usuario);
            _context.SaveChanges();
        }
        public void CrearOActualizarUsuario(Usuario usuario) {
            var existe = _context.Usuario.Find(usuario.id);
            if (existe == null) {
                CrearUsuario(usuario);
            }
            else {
                ActualizarUsuario(usuario);
            }
            _context.SaveChanges();
        }
        // busca (todos)
        public List<Usuario> ObtenerUsuarios() {
            return _context.Usuario.ToList();            
        }
        // busca (por id)
        public Usuario ObtenerPorId(int id) {            
                return _context.Usuario.Find(id);            
        }
        // borra
        public void EliminarUsuario(int id) {            
                var usuario = _context.Usuario.Find(id);
            if (usuario == null)
                return;
           _context.Usuario.Remove(usuario);
           _context.SaveChanges();                            
        }
        public int ObtenerSigienteIdUsuario()
        {
            var ultimo = _context.Usuario
                                 .OrderByDescending(x => x.id)
                                 .Select(x => x.id)
                                 .FirstOrDefault();

            return ultimo + 1;
        }
    }
}
