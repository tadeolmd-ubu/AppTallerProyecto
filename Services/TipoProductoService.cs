using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class TipoProductoService
    {
        private readonly EF.efAppDbContext _context;

        public TipoProductoService(EF.efAppDbContext context) { 
        _context = context;
        }

        public List<TipoProducto> ObtenerTiposProducto() {
        
            return _context.TipoProducto.FromSqlRaw("EXEC sp_TipoProducto @opcion = 4").ToList();
        }


    }
}
