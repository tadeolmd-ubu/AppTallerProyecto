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

        public VentaLogic(EF.efAppDbContext efAppDbContext)
        {
            // Usar la instancia proporcionada en lugar de crear una nueva
            _context = efAppDbContext ?? throw new ArgumentNullException(nameof(efAppDbContext));
            _ventaService = new VentaService(_context);
            _ventaDetalleService = new VentaDetalleService(_context);
            _inventarioService = new InventarioService(_context);
        }

                public Venta CrearVenta(Venta venta, IEnumerable<VentaDetalle> detalles)
                {
                    if (venta == null)
                        throw new ArgumentNullException(nameof(venta));
                    if (detalles == null || !detalles.Any())
                        throw new ArgumentException("La venta debe contener al menos un detalle", nameof(detalles));

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Validar stock disponible para todos los detalles antes de modificar nada
                            foreach (var det in detalles)
                            {
                                var productoValidacion = _context.Inventario.Find(det.idInventario);
                                if (productoValidacion == null)
                                    throw new Exception($"Producto con id {det.idInventario} no encontrado");

                                if (det.cantidad > 0 && productoValidacion.stockActual < det.cantidad)
                                    throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");
                            }

                            // Insertar la venta para obtener su id
                            _context.Venta.Add(venta);
                            _context.SaveChanges();

                            // Añadir detalles y descontar stock
                            foreach (var det in detalles)
                            {
                                det.idVenta = venta.id;

                                var producto = _context.Inventario.Find(det.idInventario);
                                // Revalidación por seguridad
                                if (producto == null)
                                    throw new Exception($"Producto con id {det.idInventario} no encontrado");

                                if (det.cantidad > 0 && producto.stockActual < det.cantidad)
                                    throw new Exception($"Stock insuficiente para el producto con id {det.idInventario}");

                                // Restar stock (si cantidad es positiva). Si cantidad es negativa, interpretar como devolución y sumar.
                                producto.stockActual -= det.cantidad;

                                // El producto fue obtenido por Find -> está siendo trackeado por el contexto, no es necesario marcar estado.
                                _context.VentaDetalle.Add(det);
                            }

                            _context.SaveChanges();

                            // Recalcular importes de cada detalle si el servicio proporciona esa funcionalidad
                            foreach (var det in detalles)
                            {
                                // Recalcular usando el id ya persistido
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

        // Actualizar cantidad y el importe de una venta
        public void ActualizarCantidad(int idDetalle, int nuevaCantidad)
        {
            var detalle = _context.VentaDetalle.Find(idDetalle);
            if (detalle == null)
                throw new Exception("Detalle no encontrado");

            var diferencia = nuevaCantidad - detalle.cantidad;

            var producto = _context.Inventario.Find(detalle.idInventario);
            if (producto == null)
                throw new Exception("Producto no encontrado");

            // Si se incrementa la cantidad, comprobar stock
            if (diferencia > 0 && producto.stockActual < diferencia)
                throw new Exception("Stock insuficiente");

            // Aplicar la diferencia al stock (si diferencia es negativa, esto suma stock)
            producto.stockActual -= diferencia;

            detalle.cantidad = nuevaCantidad;
            _ventaDetalleService.RecalcularImporte(detalle.id);

            _context.SaveChanges();
        }
    }
}
