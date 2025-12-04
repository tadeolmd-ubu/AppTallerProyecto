using AppTaller.Logics;
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
    /// Lógica de interacción para UcVenta.xaml
    /// </summary>
    public partial class UcVenta : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly InventarioService _inventarioService;
        private readonly ProductoService _productoService;
        private readonly VentaService _ventaService;
        private readonly ObservableCollection<PresupuestoDetalle> _detalles;
        public UcVenta()
        {
            EF.efAppDbContext context = new EF.efAppDbContext();
            _inventarioService = new InventarioService(context);
            _productoService = new ProductoService(context);
            _ventaService = new VentaService(context);
            InitializeComponent();
            _detalles = new ObservableCollection<PresupuestoDetalle>();
            dtgDetalles.ItemsSource = _detalles;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var presupuesto = new Presupuesto
                {
                    id = int.Parse(txtIdPresupuesto.Text),
                    total = Convert.ToDecimal(txtTotal.Text),
                    estatus = chkEstatus.IsChecked ?? false,
                    nota = txtNota.Text,
                    idCliente = (int)(cmbCliente.SelectedValue ?? 0),
                    idUsuario = 1001
                };
                var existe = _presupuestoService.BuscarPresupuesto(presupuesto.id);

                var logic = new PresupuestoLogic(new EF.efAppDbContext());

                if (existe != null)
                {
                    var detalles = new List<PresupuestoDetalle>();

                    foreach (var item in _detalles)
                    {
                        detalles.Add(new PresupuestoDetalle
                        {
                            id = item.id,
                            cantidad = item.cantidad,
                            precioUnitario = item.precioUnitario,
                            importe = item.importe,
                            iva = item.iva,
                            idInventario = item.idInventario,
                            idPresupuesto = presupuesto.id
                        });
                    }

                    logic.ActualizarPresupuestoConDetalles(presupuesto, detalles);

                    MessageBox.Show("Presupuesto actualizado correctamente.");
                }

                else
                {//hace que el presupuestoDetalle tenga el id que sigue en la bd
                    int nextId;
                    using (var tempCtx = new EF.efAppDbContext())
                    {
                        nextId = tempCtx.PresupuestoDetalle
                                        .OrderByDescending(x => x.id)
                                        .Select(x => x.id)
                                        .FirstOrDefault() + 1;
                    }

                    var detalles = new List<PresupuestoDetalle>();

                    foreach (var item in _detalles)
                    {
                        detalles.Add(new PresupuestoDetalle
                        {
                            id = nextId,
                            cantidad = item.cantidad,
                            precioUnitario = item.precioUnitario,
                            importe = item.importe,
                            iva = item.iva,
                            idInventario = item.idInventario,
                            idPresupuesto = presupuesto.id
                        });
                        nextId++;
                    }
                    logic.CrearPresupuestoConDetalles(presupuesto, detalles);

                    MessageBox.Show("Presupuesto guardado correctamente.");
                }
                txtIdVenta.Text = _ventaService.ObtenerSigienteIdVenta().ToString();
                LimpiarControles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR COMPLETO:\n\n" + ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }



        private void btnBuscarProducto_Click(object sender, RoutedEventArgs e)
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
                        var inventario = _inventarioService.BuscarInventario(inventarioSeleccionado.idProducto);
                        if (inventario != null)
                        {
                            AgregarProducto(inventario);
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
        private void dtgDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            var detalle = boton.DataContext as PresupuestoDetalle;

            if (detalle == null) return;

            var resultado = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                _detalles.Remove(detalle);
                RecalcularTotales();
            }

        }

        private void AgregarProducto(Inventario inventario)
        {
            // Buscar el producto relacionado al inventario
            var producto = _productoService.BuscarProductoIndividual(inventario.idProducto);
            if (producto == null)
            {
                MessageBox.Show("No se encontró el producto relacionado al inventario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // verifica si ya existe un detalle con este inventario
            var detalleExistente = _detalles.FirstOrDefault(d => d.idInventario == inventario.id);

            if (detalleExistente != null)
            {
                // Solo aumentar cantidad e importe
                detalleExistente.cantidad++;
                detalleExistente.importe = detalleExistente.cantidad * detalleExistente.precioUnitario;
                detalleExistente.iva = (int)((chkAplicarIVA.IsChecked == true)
                    ? detalleExistente.importe * 0.16m
                    : 0);
            }
            else
            {
                //precio
                var precio = producto.precio;

                var nuevoDetalle = new PresupuestoDetalle
                {
                    idInventario = inventario.id,
                    cantidad = 1,
                    precioUnitario = precio,
                    importe = precio,
                    iva = (int)((chkAplicarIVA.IsChecked == true) ? precio * 0.16m : 0)
                };

                _detalles.Add(nuevoDetalle);
            }

            dtgDetalles.Items.Refresh();
            RecalcularTotales();
        }
        private void RecalcularTotales()
        {
            decimal subtotal = _detalles.Sum(d => d.importe);
            decimal totalIva = chkAplicarIVA.IsChecked == true ? subtotal * 0.16m : 0;
            decimal total = subtotal + totalIva;
            foreach (var detalle in _detalles)
            {
                detalle.iva = (int)(chkAplicarIVA.IsChecked == true ? detalle.importe * 0.16m : 0);
            }

            dtgDetalles.Items.Refresh();
            txtTotal.Text = total.ToString("N2");
        }
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
        private void btnEliminarVenta_Click(object sender, RoutedEventArgs e)
        {
        }
        private void LimpiarControles()
        {
            txtIdVenta.Text = string.Empty;
            txtTotal.Text = "0.00";
            chkEstatus.IsChecked = false;
            txtFolio.Text = string.Empty;
            cmbCliente.SelectedIndex = -1;
            _detalles.Clear();
            dtgDetalles.Items.Refresh();
        }
    }
}