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
        private readonly ClienteService _clienteService;
        private readonly PresupuestoService _presupuestoService;
        private readonly PresupuestoLogic _presupuestoLogic;
        private readonly ObservableCollection<VentaDetalle> _detalles;
        public UcVenta()
        {
            EF.efAppDbContext context = new EF.efAppDbContext();
            _inventarioService = new InventarioService(context);
            _productoService = new ProductoService(context);
            _ventaService = new VentaService(context);
            _clienteService = new ClienteService(context);
            _presupuestoService = new PresupuestoService(context);
            _presupuestoLogic = new PresupuestoLogic(context);

            InitializeComponent();
            _detalles = new ObservableCollection<VentaDetalle>();
            dtgDetalles.ItemsSource = _detalles;

            txtIdVenta.Text = _ventaService.ObtenerSigienteIdVenta().ToString();
            txtFolio.Text = _ventaService.GenerarFolio().ToString();

            CargarClientes();

            chkAplicarIVA.Checked += (s, e) => RecalcularTotales();
            chkAplicarIVA.Unchecked += (s, e) => RecalcularTotales();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones básicas de campos requeridos
                if (string.IsNullOrWhiteSpace(txtIdVenta.Text))
                {
                    MessageBox.Show("El campo 'Id Venta' es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtIdVenta.Text.Trim(), out int idVenta))
                {
                    MessageBox.Show("El campo 'Id Venta' debe ser un número válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFolio.Text))
                {
                    MessageBox.Show("El campo 'Folio' es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtFolio.Text.Trim(), out int folio))
                {
                    MessageBox.Show("El campo 'Folio' debe ser un número válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbCliente.SelectedValue == null || !int.TryParse(cmbCliente.SelectedValue.ToString(), out int idCliente) || idCliente == 0)
                {
                    MessageBox.Show("Seleccione un cliente válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTotal.Text))
                {
                    MessageBox.Show("El total no puede estar vacío. Asegúrese de tener productos en la venta.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!decimal.TryParse(txtTotal.Text.Trim(), out decimal total))
                {
                    MessageBox.Show("El campo 'Total' debe ser un número válido.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_detalles == null || _detalles.Count == 0)
                {
                    MessageBox.Show("Agregue al menos un producto a la venta.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validar contenido de cada detalle
                foreach (var d in _detalles)
                {
                    if (d.cantidad <= 0)
                    {
                        MessageBox.Show("Cada producto debe tener una cantidad mayor a cero.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (d.precioUnitario < 0)
                    {
                        MessageBox.Show("El precio unitario no puede ser negativo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Construir objeto venta seguro
                var venta = new Venta
                {
                    id = idVenta,
                    total = total,
                    estatus = chkEstatus.IsChecked == true,
                    folio = folio,
                    idUsuario = 1001,
                    idCliente = idCliente,
                };

                // Lógica para crear la venta con detalles (obtener siguiente id para VentaDetalle)
                var logic = new VentaLogic(new EF.efAppDbContext());

                int nextId;
                using (var tempCtx = new EF.efAppDbContext())
                {
                    nextId = tempCtx.VentaDetalle
                                    .OrderByDescending(x => x.id)
                                    .Select(x => x.id)
                                    .FirstOrDefault() + 1;
                }

                var detalles = new List<VentaDetalle>();
                foreach (var item in _detalles)
                {
                    detalles.Add(new VentaDetalle
                    {
                        id = nextId,
                        cantidad = item.cantidad,
                        precioUnitario = item.precioUnitario,
                        importe = item.importe,
                        iva = item.iva,
                        idInventario = item.idInventario,
                        idVenta = venta.id
                    });
                    nextId++;
                }

                logic.CrearVenta(venta, detalles);
                MessageBox.Show("Venta guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Limpiar controles y actualizar id/folio siguientes
                LimpiarControles();
                txtIdVenta.Text = _ventaService.ObtenerSigienteIdVenta().ToString();
                txtFolio.Text = _ventaService.GenerarFolio().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR COMPLETO:\n\n" + ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var presupuestos = _presupuestoService.ObtenerPresupuestos();
                string[] columnas = { "id", "total", "estatus", "nota", "idCliente", "idUsuario", "fechaCreacion", "fechaModificacion" };

                var ventana = new FrmBusqueda("Búsqueda de Presupuestos", presupuestos, columnas);

                if (ventana.ShowDialog() == true)
                {
                    if (ventana.seleccionado is Presupuesto presupuestoSeleccionado)
                    {
                        CargarDetallesPresupuesto(presupuestoSeleccionado.id);
                        cmbCliente.SelectedValue = presupuestoSeleccionado.idCliente;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar Presupuesto: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarDetallesPresupuesto(int idPresupuesto)
        {
            try
            {
                var detallesBD = _presupuestoLogic.ObtenerDetallesPorPresupuesto(idPresupuesto);

                _detalles.Clear();

                foreach (var d in detallesBD)
                {
                    // Mapear PresupuestoDetalle -> VentaDetalle
                    var nuevoDetalle = new VentaDetalle
                    {
                        // Asumimos que PresupuestoDetalle tiene estas propiedades.
                        idInventario = d.idInventario,
                        cantidad = d.cantidad,
                        precioUnitario = d.precioUnitario,
                        importe = d.importe,
                        // Recalcular IVA si está marcada la casilla, si no usar el IVA del presupuesto (si existe)
                        iva = (int)((chkAplicarIVA.IsChecked == true) ? d.importe * 0.16m : (d.iva))
                    };

                    _detalles.Add(nuevoDetalle);
                }

                dtgDetalles.ItemsSource = _detalles;
                dtgDetalles.Items.Refresh();

                RecalcularTotales();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar detalles del presupuesto: " + ex.Message);
            }
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
            var detalle = boton.DataContext as VentaDetalle;

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

                var nuevoDetalle = new VentaDetalle
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
            var detalle = button.Tag as VentaDetalle;

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
            var detalle = button.Tag as VentaDetalle;

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
        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteService.ObtenerClientes();

                if (clientes == null || clientes.Count == 0)
                {
                    MessageBox.Show("No se encontraron empresas.");
                    return;
                }
                cmbCliente.ItemsSource = clientes;
                cmbCliente.SelectedValuePath = "id";
                cmbCliente.DisplayMemberPath = "nombre";

                cmbCliente.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empresas: " + ex.Message);
            }
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
            txtIdVenta.Text = _ventaService.ObtenerSigienteIdVenta().ToString();
            txtFolio.Text = _ventaService.GenerarFolio().ToString();
        }
    }
}