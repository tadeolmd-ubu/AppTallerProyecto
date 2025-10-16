using AppTaller.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AppTaller.Model;

namespace AppTaller.Services
{
    internal class RolService
    {
        private readonly EF.efAppDbContext _context;

        public RolService(EF.efAppDbContext context)
        {
            _context = context;
        }

        public List<catRol> ObtenerRoles(){
                return _context.catRol.ToList();
            
        }
    }
}
