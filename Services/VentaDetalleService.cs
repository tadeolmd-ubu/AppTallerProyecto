using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Services
{
    internal class VentaDetalleService
    {
        private readonly EF.efAppDbContext _context;
        public VentaDetalleService(EF.efAppDbContext context){
            _context = context;
        }
        public void CrearVentaDetalle(VentaDetalle ventaDetalle) {
            if (ventaDetalle.id == 0)
                ventaDetalle.id = _context.SiguienteId("VentaDetalle");
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_VentaDetalle @opcion = 1, @id = {0}, @cantidad = {1}, @importe = {2}, @iva = {3}, @idInventario = {4}, @idVenta = {5}, @precioUnitario = {6}",
                ventaDetalle.id, ventaDetalle.cantidad, ventaDetalle.importe, ventaDetalle.iva,
                ventaDetalle.idInventario, ventaDetalle.idVenta, ventaDetalle.precioUnitario);
        }
        public void EliminarDetallesPorVenta(int idVenta)
        {
            var detalles = _context.VentaDetalle
                .Where(d => d.idVenta == idVenta)
                .ToList();

            foreach (var d in detalles)
            {
                var producto = _context.Inventario.Find(d.idInventario);
                if (producto != null)
                    producto.stockActual += d.cantidad;
                _context.Database.ExecuteSqlRaw("EXEC sp_VentaDetalle @opcion = 3, @id = {0}", d.id);
            }
        }
        public List<VentaDetalle> ObtenerDetallesPorVenta(int idVenta)
        {
            return _context.VentaDetalle
                .FromSqlRaw("EXEC sp_VentaDetalle @opcion = 4, @idVenta = {0}", idVenta)
                .ToList();
        }

        public void RecalcularImporte(int idDetalle)
        {
            var detalle = _context.VentaDetalle.Find(idDetalle);
            if (detalle == null)
                throw new Exception("Detalle no encontrado");

            detalle.importe = detalle.cantidad * detalle.precioUnitario;
            detalle.iva = detalle.importe * 0.16m;

            _context.SaveChanges();
        }
        public bool ValidarStock(int idProducto, int cantidad)
        {
            var producto = _context.Inventario.Find(idProducto);
            if (producto == null) return false;

            return producto.stockActual >= cantidad;
        }
        public decimal CalcularSubtotal(int idVenta)
        {
            return _context.VentaDetalle
                .Where(d => d.idVenta == idVenta)
                .Sum(d => d.importe);
        }

        public decimal CalcularIVATotal(int idVenta)
        {
            return _context.VentaDetalle
                .Where(d => d.idVenta == idVenta)
                .Sum(d => d.iva);
        }
        public decimal CalcularTotal(int idVenta)
        {
            return CalcularSubtotal(idVenta) + CalcularIVATotal(idVenta);
        }
    }
}
