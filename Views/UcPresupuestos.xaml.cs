using AppTaller.EF;
using AppTaller.Logics;
using AppTaller.Model;
using AppTaller.Services;
using Microsoft.EntityFrameworkCore;
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
        private readonly ObservableCollection<PresupuestoDetalle> _detalles;
        private readonly PresupuestoLogic _presupuestoLogic;
        private readonly ClienteService _clienteService;
        private readonly PresupuestoDetalleService _presupuestoDetalleService;

        public UcPresupuestos()
        {
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            _inventarioService = new InventarioService(_context);
            _presupuestoLogic = new PresupuestoLogic(_context);
            _clienteService = new ClienteService(_context);
            _presupuestoDetalleService = new PresupuestoDetalleService(_context);
            InitializeComponent();

            CargarClientes();
            _detalles = new ObservableCollection<PresupuestoDetalle>();
            dtgDetalles.ItemsSource = _detalles;

            // Suscribirse al evento del checkbox IVA
            chkAplicarIVA.Checked += (s, e) => RecalcularTotales();
            chkAplicarIVA.Unchecked += (s, e) => RecalcularTotales();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try{
                var presupuesto = new Presupuesto{
                    id =  int.Parse(txtIdPresupuesto.Text),
                    total = Convert.ToDecimal(txtTotal.Text),
                    estatus = chkEstatus.IsChecked ?? false,
                    nota = txtNota.Text,
                    idCliente = (int)(cmbCliente.SelectedValue ?? 0),
                    idUsuario = 1001
                };
                int nextId;
                using (var tempCtx = new EF.efAppDbContext()){
                    nextId = tempCtx.PresupuestoDetalle
                                    .OrderByDescending(x => x.id)
                                    .Select(x => x.id)
                                    .FirstOrDefault() + 1;
                }
                var detalles = new List<PresupuestoDetalle>();

                foreach (var item in _detalles) {
                    detalles.Add(new PresupuestoDetalle{
                        id = nextId,            
                        cantidad = item.cantidad,
                        precioUnitario = item.precioUnitario,
                        importe = item.importe,
                        iva = item.iva,
                        idProducto = item.idProducto,
                        idPresupuesto = presupuesto.id
                    });

                    nextId++; 
                }
                var logic = new PresupuestoLogic(new EF.efAppDbContext());
                logic.CrearPresupuestoConDetalles(presupuesto, detalles);

                MessageBox.Show("Presupuesto guardado correctamente.");
            }
            catch (Exception ex){
                MessageBox.Show("ERROR COMPLETO:\n\n" +ex.ToString());
            }

        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var botton = sender as Button;
            var detalle = botton.Tag as PresupuestoDetalle;

            if (detalle != null){
                var resultado = MessageBox.Show($"¿Está seguro de eliminar este producto?","Confirmar eliminación",MessageBoxButton.YesNo,MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes){
                    _detalles.Remove(detalle);
                    RecalcularTotales();
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void lvDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnBuscarProducto_Click_1(object sender, RoutedEventArgs e)
        {
            try{
                var inventarios = _inventarioService.ObtenerInventarios();
                string[] columnas = { "id", "idProducto", "idAlmacen", "stockActual" };
                var ventana = new FrmBusqueda("Búsqueda de Inventarios", inventarios, columnas);

                if (ventana.ShowDialog() == true){
                    if (ventana.seleccionado is Inventario inventarioSeleccionado){
                        // Buscar el producto completo
                        var producto = _productoService.BuscarProductoIndividual(inventarioSeleccionado.idProducto);

                        if (producto != null){
                            AgregarProducto(producto);
                        }
                        else{
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
            var detalleExistente = _detalles.FirstOrDefault(d => d.idProducto == producto.id);

            if (detalleExistente != null){
                detalleExistente.cantidad++;
                detalleExistente.importe = detalleExistente.cantidad * detalleExistente.precioUnitario;
                detalleExistente.iva = (int)(chkAplicarIVA.IsChecked == true ? detalleExistente.importe * 0.16m : 0);
            }
            else{
                var nuevoDetalle = new PresupuestoDetalle{
                    idProducto = producto.id,   
                    cantidad = 1,
                    precioUnitario = producto.precio,
                    importe = producto.precio,
                    iva = (int)(chkAplicarIVA.IsChecked == true ? producto.precio * 0.16m : 0)
                };

                _detalles.Add(nuevoDetalle);
            }

            dtgDetalles.Items.Refresh();
            RecalcularTotales();
        }
        private void btnAumentar_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var detalle = button.Tag as PresupuestoDetalle;

            if (detalle != null){
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

            if (detalle != null && detalle.cantidad > 1){
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
            foreach (var detalle in _detalles){
                detalle.iva = (int)(chkAplicarIVA.IsChecked == true ? detalle.importe * 0.16m : 0);
            }

            dtgDetalles.Items.Refresh();
            txtTotal.Text = total.ToString("N2");
        }

        private void CargarClientes()
        {
            try{
                var clientes = _clienteService.ObtenerClientes();

                if (clientes == null || clientes.Count == 0){
                    MessageBox.Show("No se encontraron empresas.");
                    return;
                }
                cmbCliente.ItemsSource = clientes;
                cmbCliente.SelectedValuePath = "id";
                cmbCliente.DisplayMemberPath = "id";

                cmbCliente.SelectedIndex = 0;
            }
            catch (Exception ex){
                MessageBox.Show("Error al cargar empresas: " + ex.Message);
            }
        }

        private void txtTotal_TextChanged(object sender, TextChangedEventArgs e)
        {   
        }

        private void dtgDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
