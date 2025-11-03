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
    /// Lógica de interacción para UcRegistroStock.xaml
    /// </summary>
    public partial class UcRegistroStock : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly ProductoService _productoService;
        private readonly AlmacenService _almacenService;
        private readonly TipoMovimientoService _tipoMovimientoService;
        public UcRegistroStock()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _productoService = new ProductoService(_context);
            _almacenService = new AlmacenService(_context);
            _tipoMovimientoService = new TipoMovimientoService(_context);
            CargarProducto();
            CargarAlmacen();
            CargarTipoMovimiento();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
