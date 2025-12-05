using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

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

                            _context.Venta.Add(venta);
                            _context.SaveChanges();

                            foreach (var det in detalles){
                                det.idVenta = venta.id;

                                var producto = _context.Inventario.Find(det.idInventario);
                                
                                if (producto == null)
                                    throw new Exception($"Producto con id {det.idInventario} no encontrado");

                                if (det.cantidad > 0 && producto.stockActual < det.cantidad)
                                    throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");

                                producto.stockActual -= det.cantidad;

                                _context.VentaDetalle.Add(det);
                            }

                            _context.SaveChanges();

                            foreach (var det in detalles){
                                _ventaDetalleService.RecalcularImporte(det.id);
                            }

                            _context.SaveChanges();
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
