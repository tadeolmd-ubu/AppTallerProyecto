using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class AlmacenService
    {
        private readonly EF.efAppDbContext _context;
        public AlmacenService(EF.efAppDbContext context) {
        _context = context; 
        }
        public List<catAlmacen> ObtenerAlmacenes() {
            return _context.catAlmacen.FromSqlRaw("EXEC sp_catAlmacen @opcion = 4").ToList();
        }
    }
}
