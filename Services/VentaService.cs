using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class VentaService
    {
        private readonly EF.efAppDbContext _context;

        public VentaService(EF.efAppDbContext context){
            _context = context;
        }

        public void CrearVenta(Venta venta) { 
        _context.Venta.Add(venta);
        }
        public void BuscarVenta(int id) {
        _context.Venta.Find(id);
        }

        public List <Venta> ObtenerVentas(){
            return _context.Venta.ToList();
        }

        public int GenerarFolio(){
            var ultimo = _context.Venta
                                 .OrderByDescending(x => x.folio)
                                 .Select(x => x.folio)
                                 .FirstOrDefault();

            return ultimo + 1;
        }
        public int ObtenerSigienteIdVenta(){
            var ultimo = _context.Venta
                                 .OrderByDescending(x => x.id)
                                 .Select(x => x.id)
                                 .FirstOrDefault();

            return ultimo + 1;
        }

    }
}
