using AppTaller.Logics;
using AppTaller.Model;
using AppTaller.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para UcRegistroStock.xaml
    /// </summary>
    public partial class UcRegistroStock : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly ProductoService _productoService;
        private readonly AlmacenService _almacenService;
        private readonly TipoMovimientoService _tipoMovimientoService;
        private readonly ReferenciaService _referenciaService;
        private readonly InventarioService _inventarioService;
        private readonly StockLogic _stockLogic ;
        public UcRegistroStock()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            _almacenService = new AlmacenService(_context);
            _tipoMovimientoService = new TipoMovimientoService(_context);
            _referenciaService = new ReferenciaService(_context);
            _inventarioService = new InventarioService(_context);
            _stockLogic = new StockLogic(_context);
            CargarProducto();
            CargarAlmacen();
            CargarTipoMovimiento();
            CargarReferencias();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validaciones
                if (cmbProducto.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecciona un producto.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbAlmacen.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecciona un almacén.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbTipoMovimiento.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecciona el tipo de movimiento.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbReferencia.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecciona la referencia.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out int cantidad))
                {
                    MessageBox.Show("Ingresa una cantidad válida.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener valores
                int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);
                int idAlmacen = Convert.ToInt32(cmbAlmacen.SelectedValue);
                int idTipoMovimiento = Convert.ToInt32(cmbTipoMovimiento.SelectedValue);
                int idReferencia = Convert.ToInt32(cmbReferencia.SelectedValue);

                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    if (idTipoMovimiento == 1) // Entrada
                    {
                        MessageBox.Show("Debes indicar el ID para crear el inventario.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Para ajuste necesitas ingresar el ID del inventario existente.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                int idInventario = Convert.ToInt32(txtId.Text);

                // Llamar al servicio
                _stockLogic.RegistrarEntradaOAjuste(idInventario,
                                                   idProducto,
                                                   idAlmacen,
                                                   cantidad,
                                                   idTipoMovimiento,
                                                   idReferencia);

                MessageBox.Show("Movimiento guardado exitosamente ", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            catch (Exception ex)
            {
                string error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(error, "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }                     
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void CargarProducto()
        {
            try
            {
                var producto = _productoService.ObtenerProductos();

                if (producto == null || producto.Count == 0)
                {
                    MessageBox.Show("No se encontraron el producto.");
                    return;
                }
                cmbProducto.ItemsSource = producto;
                cmbProducto.SelectedValuePath = "id";
                cmbProducto.DisplayMemberPath = "id";

                cmbProducto.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message);

            }
        }
        private void CargarAlmacen()
        {
            try
            {
                var almacen = _almacenService.ObtenerAlmacenes();

                if (almacen == null || almacen.Count == 0)
                {
                    MessageBox.Show("No se encontraron el almacen.");
                    return;
                }
                cmbAlmacen.ItemsSource = almacen;
                cmbAlmacen.SelectedValuePath = "id";
                cmbAlmacen.DisplayMemberPath = "id";

                cmbAlmacen.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los almacenes: " + ex.Message);

            }
        }
        private void CargarTipoMovimiento()
        {
            try
            {
                var tipoMovimiento = _tipoMovimientoService.ObtenerTiposMovimientos();

                if (tipoMovimiento == null || tipoMovimiento.Count == 0)
                {
                    MessageBox.Show("No se encontraron los tipos de movimiento.");
                    return;
                }
                cmbTipoMovimiento.ItemsSource = tipoMovimiento;
                cmbTipoMovimiento.SelectedValuePath = "id";
                cmbTipoMovimiento.DisplayMemberPath = "id";

                cmbTipoMovimiento.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de movimiento: " + ex.Message);

            }
        }
        private void CargarReferencias()
        {
            try
            {
                var referencia = _referenciaService.ObtenerReferencias();

                if (referencia == null || referencia.Count == 0)
                {
                    MessageBox.Show("No se encontraron las referencias de movimientos.");
                    return;
                }
                cmbReferencia.ItemsSource = referencia;
                cmbReferencia.SelectedValuePath = "id";
                cmbReferencia.DisplayMemberPath = "id";

                cmbReferencia.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de movimiento: " + ex.Message);

            }
        }

        private void BtnSumar_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(txtCantidad.Text);
            txtCantidad.Text = (val + 1).ToString();
        }

        private void BtnRestar_Click(object sender, RoutedEventArgs e)
        {
            int val = int.Parse(txtCantidad.Text);
            if (val > 0) txtCantidad.Text = (val - 1).ToString();
        }
        private void SoloNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            try{
                var productos = _productoService.ObtenerProductos();

                string[] columnas = { "id", "nombre", "precio", "estatus", "idMarca", "idTipoProducto", "fechaCreacion", "fechaActualizacion" };

                var ventana = new FrmBusqueda("Búsqueda de Clientes", productos, columnas);

                if (ventana.ShowDialog() == true){
                    if (ventana.seleccionado is Producto productoSeleccionado){
                        cmbProducto.SelectedValue = productoSeleccionado.id;
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
            }
        }

        private void btnRegistros_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inventarios= _inventarioService.ObtenerInventarios();

                string[] columnas = { "id", "idProducto", "idAlmacen", "stockActual", "fechaUltimaActualizacion"};

                var ventana = new FrmBusqueda("Búsqueda de Inventarios", inventarios, columnas);

                if (ventana.ShowDialog() == true)
                {
                    if (ventana.seleccionado is Inventario inventarioSeleccionado)
                    {
                        cmbProducto.SelectedValue = inventarioSeleccionado.idProducto;
                        cmbAlmacen.SelectedValue = inventarioSeleccionado.idAlmacen;
                        txtId.Text = inventarioSeleccionado.id.ToString();
                        txtCantidad.Text = inventarioSeleccionado.stockActual.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
            }
        }
    }
}
