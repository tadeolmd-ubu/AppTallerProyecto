using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class MarcaService
    {
        private readonly EF.efAppDbContext _context;

        public MarcaService(EF.efAppDbContext context){
            _context = context;
        }
        public List<Model.catMarca> ObtenerTodasLasMarcas(){
            return _context.catMarcas.FromSqlRaw("EXEC sp_catMarca @opcion = 4").ToList();
        }
    }
}
