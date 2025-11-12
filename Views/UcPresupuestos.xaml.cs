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
    /// Lógica de interacción para UcPresupuestos.xaml
    /// </summary>
    public partial class UcPresupuestos : UserControl
    {
        private readonly EF.efAppDbContext _context;

        private readonly ProductoService _productoService;

        public UcPresupuestos()
        {
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBuscarProducto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvDetalles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnBuscarProducto_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var productos = _productoService.ObtenerProductos();

                string[] columnas = { "id", "nombre", "precio", "estatus", "idMarca", "idTipoProducto", "fechaCreacion", "fechaActualizacion" };

                var ventana = new FrmBusqueda("Búsqueda de Productos", productos, columnas);

                if (ventana.ShowDialog() == true)
                {
                    if (ventana.seleccionado is Producto productoSeleccionado)
                    {
                        txtIdProducto.Text = productoSeleccionado.id.ToString();
                        CargarDatosProducto(productoSeleccionado);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar Productos: " + ex.Message);
            }
        }

        private void CargarDatosProducto(Producto producto)
        {
            try{
                dtgDetalles.Items.Clear();

                // Agregar directamente sin ItemsSource
                dtgDetalles.Items.Add(producto);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos del producto: " + ex.Message);
            }
        }

    }
}
