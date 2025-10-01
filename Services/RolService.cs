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
        public List<catRol> ObtenerRoles()
        {
            using (var context = new efAppDbContext())
            {
                return context.catRol.ToList();
            }
        }
    }
}
