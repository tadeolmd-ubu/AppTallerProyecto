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
            if (venta.id == 0)
                venta.id = _context.SiguienteId("Venta");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_Venta @opcion = 1, @id = {0}, @total = {1}, @estatus = {2}, @idUsuario = {3}, @idCliente = {4}, @folio = {5}",
                venta.id, venta.total, venta.estatus, venta.idUsuario, venta.idCliente, venta.folio);
        }
        public Venta BuscarVenta(int id) {
            return _context.Venta.FromSqlRaw("EXEC sp_Venta @opcion = 5, @id = {0}", id).AsEnumerable().FirstOrDefault();
        }

        public List <Venta> ObtenerVentas(){
            return _context.Venta.FromSqlRaw("EXEC sp_Venta @opcion = 4").ToList();
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
