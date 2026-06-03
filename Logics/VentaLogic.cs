using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AppTaller.Logics
{
    internal class VentaLogic
    {
        private readonly EF.efAppDbContext _context;
        private readonly VentaService _ventaService;
        private readonly VentaDetalleService _ventaDetalleService;
        private readonly InventarioService _inventarioService;
        public VentaLogic(EF.efAppDbContext efAppDbContext){
            _context = efAppDbContext ?? throw new ArgumentNullException(nameof(efAppDbContext));
            _ventaService = new VentaService(_context);
            _ventaDetalleService = new VentaDetalleService(_context);
            _inventarioService = new InventarioService(_context);
        }
                public Venta CrearVenta(Venta venta, IEnumerable<VentaDetalle> detalles){
                    if (venta == null)
                        throw new ArgumentNullException(nameof(venta));
                    if (detalles == null || !detalles.Any())
                        throw new ArgumentException("La venta debe contener al menos un detalle", nameof(detalles));

                    using (var transaction = _context.Database.BeginTransaction()){
                        try{
                            foreach (var det in detalles){
                                var productoValidacion = _context.Inventario.Find(det.idInventario);
                                if (productoValidacion == null)
                                    throw new Exception($"Producto con id {det.idInventario} no encontrado");

                                if (det.cantidad > 0 && productoValidacion.stockActual < det.cantidad)
                                    throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");
                            }

                            _ventaService.CrearVenta(venta);

                            foreach (var det in detalles){
                                det.idVenta = venta.id;

                                var producto = _context.Inventario.Find(det.idInventario);

                                if (producto == null)
                                    throw new Exception($"Producto con id {det.idInventario} no encontrado");

                                if (det.cantidad > 0 && producto.stockActual < det.cantidad)
                                    throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");

                                producto.stockActual -= det.cantidad;

                                _context.Database.ExecuteSqlRaw(
                                    "EXEC sp_Inventario @opcion = 2, @id = {0}, @idProducto = {1}, @idAlmacen = {2}, @stockActual = {3}",
                                    producto.id, producto.idProducto, producto.idAlmacen, producto.stockActual);

                                _ventaDetalleService.CrearVentaDetalle(det);
                            }

                            foreach (var det in detalles){
                                _ventaDetalleService.RecalcularImporte(det.id);
                            }

                            transaction.Commit();
                            return venta;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
        }
    }
}
