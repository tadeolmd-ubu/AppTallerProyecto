using AppTaller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaller.Services
{
    internal class VentaDetalleService
    {
        private readonly EF.efAppDbContext _context;
        public VentaDetalleService(EF.efAppDbContext context){
            _context = context;
        }

        public void CrearVentaDetalle(VentaDetalle ventaDetalle) {
            _context.VentaDetalle.Add(ventaDetalle);
        }
        public void EliminarDetallesPorVenta(int idVenta)
        {
            var detalles = _context.VentaDetalle
                .Where(d => d.idVenta == idVenta)
                .ToList();

            foreach (var d in detalles)
            {
                var producto = _context.Inventario.Find(d.idInventario);
                producto.stockActual += d.cantidad;
            }

            _context.VentaDetalle.RemoveRange(detalles);
            _context.SaveChanges();
        }
        public List<VentaDetalle> ObtenerDetallesPorVenta(int idVenta)
        {
            return _context.VentaDetalle
                .Where(d => d.idVenta == idVenta)
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
