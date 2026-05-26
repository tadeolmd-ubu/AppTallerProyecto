using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
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
        public Venta BuscarVenta(int id) {
        return _context.Venta.Find(id);
        }

        public List <Venta> ObtenerVentas(){
            return _context.Venta.ToList();
        }

        public int GenerarFolio(){
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "EXEC sp_ObtenerSiguienteFolio";
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return Convert.ToInt32(reader["siguienteFolio"]);
                }
            }
            return 1;
        }
        public int ObtenerSigienteIdVenta(){
            return _context.SiguienteId("Venta");
        }

    }
}
