using AppTaller.Model;
using AppTaller.Services;
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
    /// Lógica de interacción para UcProductos.xaml
    /// </summary>
    public partial class UcProductos : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly ProductoService _productoService;
        private readonly MarcaService _marcaService;
        private readonly TipoProductoService _tipoProductoService;

        public UcProductos()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            _marcaService = new MarcaService(_context);
            _tipoProductoService = new TipoProductoService(_context);
            CargarMarcas();
            CargarTipoProducto();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(txtIdProducto.Text) ||
                    string.IsNullOrWhiteSpace(txtNombreProducto.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                    cmbMarca.SelectedIndex < 0 ||
                    cmbTipoProducto.SelectedIndex < 0){
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.",
                                    "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!int.TryParse(txtIdProducto.Text, out int idProducto)){
                    MessageBox.Show("Por favor, ingrese un valor numérico válido el id del producto.",
                                    "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio)) {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido para el precio.",
                                    "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(cmbMarca.SelectedValue?.ToString(), out int idMarca) ||
                    !int.TryParse(cmbTipoProducto.SelectedValue?.ToString(), out int idTipoProducto)){
                    MessageBox.Show("Por favor, seleccione valores válidos para Marca y Tipo de Producto.",
                                    "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Producto producto = new Producto{
                    id = idProducto,
                    nombre = txtNombreProducto.Text.Trim(),
                    precio = precio,
                    estatus = chkEstatus.IsChecked ?? false,
                    idMarca = idMarca,
                    idTipoProducto = idTipoProducto 
                };

                _productoService.CrearOActualizarProducto(producto);

                LimpiarCampos();
                MessageBox.Show("Operación exitosa", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) {
                MessageBox.Show("Error al guardar o modificar: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        

        private void LimpiarCampos(){
            txtIdProducto.Text = "";
            txtIdProducto.Text = "";
            txtNombreProducto.Text = "";
            txtPrecio.Text = "";
            cmbMarca.SelectedIndex = -1;
            cmbTipoProducto.SelectedIndex = -1;
            chkEstatus.IsChecked = false;
        }
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try{
                if (int.TryParse(txtIdProducto.Text, out int id)){
                    _productoService.EliminarProducto(id);

                    MessageBox.Show("Operación exitosa", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimpiarCampos();
                }
            }
            catch (Exception ex){
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                MessageBox.Show("Error detallado:\n" + inner.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try{                
                var productos = _productoService.ObtenerProductos(); 

                string[] columnas = { "id", "nombre", "precio", "estatus", "idMarca", "idTipoProducto", "fechaCreacion", "fechaActualizacion"};

                var ventana = new FrmBusqueda("Búsqueda de Clientes", productos, columnas);

                if (ventana.ShowDialog() == true){
                    if (ventana.seleccionado is Producto productoSeleccionado){
                        txtIdProducto.Text = productoSeleccionado.id.ToString();
                        txtNombreProducto.Text = productoSeleccionado.nombre;
                        txtPrecio.Text = productoSeleccionado.precio.ToString();
                        chkEstatus.IsChecked = productoSeleccionado.estatus;
                        cmbMarca.Text = productoSeleccionado.idMarca.ToString();
                        cmbTipoProducto.Text = productoSeleccionado.idTipoProducto.ToString();
                        lblFechaCreacion.Content = productoSeleccionado.fechaCreacion;
                        lblFechaActualizacion.Content = productoSeleccionado.fechaActualizacion;
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show("Error al buscar Productos: " + ex.Message);
            }
        }

        private void CargarMarcas(){
            try{
                var marcas = _marcaService.ObtenerTodasLasMarcas();

                if (marcas == null || marcas.Count == 0){
                    MessageBox.Show("No se encontraron marcas.");
                    return;
                }
                cmbMarca.ItemsSource = marcas;
                cmbMarca.SelectedValuePath = "id";
                cmbMarca.DisplayMemberPath = "id";

                cmbMarca.SelectedIndex = 0;
            }
            catch (Exception ex){
                MessageBox.Show("Error al cargar marcas: " + ex.Message);
            }
        }
        private void CargarTipoProducto()
        {
            try{
                var tipo = _tipoProductoService.ObtenerTiposProducto();

                if (tipo == null || tipo.Count == 0){
                    MessageBox.Show("No se encontraron Tipos de producto.");
                    return;
                }
                cmbTipoProducto.ItemsSource = tipo;
                cmbTipoProducto.SelectedValuePath = "id";
                cmbTipoProducto.DisplayMemberPath = "id";

                cmbMarca.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de productos: " + ex.Message);
            }
        }
    }
}
