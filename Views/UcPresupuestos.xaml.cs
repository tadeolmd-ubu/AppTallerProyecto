using AppTaller.Model;
using AppTaller.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppTaller.Views
{
    /// <summary>
    /// Lógica de interacción para UcPresupuestos.xaml
    /// </summary>
    public partial class UcPresupuestos : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly ProductoService _productoService;
        private readonly InventarioService _inventarioService;
        private ObservableCollection<PresupuestoDetalle> _detalles;

        public UcPresupuestos()
        {
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            _inventarioService = new InventarioService(_context);
            InitializeComponent();

            _detalles = new ObservableCollection<PresupuestoDetalle>();
            dtgDetalles.ItemsSource = _detalles;

            // Suscribirse al evento del checkbox IVA
            chkAplicarIVA.Checked += (s, e) => RecalcularTotales();
            chkAplicarIVA.Unchecked += (s, e) => RecalcularTotales();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Implementar guardar
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            // Implementar buscar
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detalle = button.Tag as PresupuestoDetalle;

            if (detalle != null)
            {
                var resultado = MessageBox.Show(
                    $"¿Está seguro de eliminar este producto?",
                    "Confirmar eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    _detalles.Remove(detalle);
                    RecalcularTotales();
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, RoutedEventArgs e)
        {
            // Vacío - se usa btnBuscarProducto_Click_1
        }

        private void lvDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Evento obsoleto del ListView
        }

        private void btnBuscarProducto_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var inventarios = _inventarioService.ObtenerInventarios();
                string[] columnas = { "id", "idProducto", "idAlmacen", "stockActual" };
                var ventana = new FrmBusqueda("Búsqueda de Inventarios", inventarios, columnas);

                if (ventana.ShowDialog() == true)
                {
                    if (ventana.seleccionado is Inventario inventarioSeleccionado)
                    {
                        // Buscar el producto completo
                        var producto = _productoService.BuscarProductoIndividual(inventarioSeleccionado.idProducto);

                        if (producto != null)
                        {
                            AgregarProducto(producto);
                        }
                        else
                        {
                            MessageBox.Show("Producto no encontrado", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar Inventarios: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AgregarProducto(Producto producto)
        {
            // Verificar si el producto ya existe en el detalle
            var detalleExistente = _detalles.FirstOrDefault(d => d.idProducto == producto.id);

            if (detalleExistente != null)
            {
                // Si existe, aumentar la cantidad
                detalleExistente.cantidad++;
                detalleExistente.importe = detalleExistente.cantidad * detalleExistente.precioUnitario;
                detalleExistente.iva = (int)(chkAplicarIVA.IsChecked == true ? detalleExistente.importe * 0.16m : 0);
            }
            else
            {
                // Si no existe, agregarlo nuevo
                var nuevoDetalle = new PresupuestoDetalle
                {
                    idProducto = producto.id,
                    
                    cantidad = 1,
                    precioUnitario = producto.precio,
                    importe = producto.precio,
                    iva = (int)(chkAplicarIVA.IsChecked == true ? producto.precio * 0.16m : 0)
                };

                _detalles.Add(nuevoDetalle);
            }

            // Actualizar el DataGrid y recalcular totales
            dtgDetalles.Items.Refresh();
            RecalcularTotales();
        }

        // Botón Aumentar cantidad
        private void btnAumentar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detalle = button.Tag as PresupuestoDetalle;

            if (detalle != null)
            {
                detalle.cantidad++;
                detalle.importe = detalle.cantidad * detalle.precioUnitario;
                detalle.iva = (int)(chkAplicarIVA.IsChecked == true ? detalle.importe * 0.16m : 0);

                dtgDetalles.Items.Refresh();
                RecalcularTotales();
            }
        }

        // Botón Disminuir cantidad
        private void btnDisminuir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detalle = button.Tag as PresupuestoDetalle;

            if (detalle != null && detalle.cantidad > 1)
            {
                detalle.cantidad--;
                detalle.importe = detalle.cantidad * detalle.precioUnitario;
                detalle.iva = (int)(chkAplicarIVA.IsChecked == true ? detalle.importe * 0.16m : 0);

                dtgDetalles.Items.Refresh();
                RecalcularTotales();
            }
        }

        // Recalcular totales
        private void RecalcularTotales()
        {
            decimal subtotal = _detalles.Sum(d => d.importe);
            decimal totalIva = chkAplicarIVA.IsChecked == true ? subtotal * 0.16m : 0;
            decimal total = subtotal + totalIva;

            // Actualizar IVA en cada detalle
            foreach (var detalle in _detalles)
            {
                detalle.iva = (int)(chkAplicarIVA.IsChecked == true ? detalle.importe * 0.16m : 0);
            }

            dtgDetalles.Items.Refresh();
            txtTotal.Text = total.ToString("N2");
        }

        private void txtTotal_TextChanged(object sender, TextChangedEventArgs e)
        {   
        }

        private void dtgDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
