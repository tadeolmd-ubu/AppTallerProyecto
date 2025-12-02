using AppTaller.Logics;
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
    /// Lógica de interacción para UcProveedores.xaml
    /// </summary>
    public partial class UcProveedores : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly EmpresaService _empresaService;
        private readonly ProveedorService _proveedorService;
        private readonly ProveedorLogic _proveedorLogic;
        private readonly DireccionService _direccionService;
        public UcProveedores()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _empresaService = new EmpresaService(_context);
            _proveedorService = new ProveedorService(_context);
            _direccionService = new DireccionService(_context);
            _proveedorLogic = new ProveedorLogic(_context, new DireccionService(_context), new ProveedorService(_context));

            CargarEmpresas();
            CargarSiguienteIdProveedor();
            CargarSiguienteIdDireccion();
        }

        private void btnGuardar_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validación de campos obligatorios
                if (string.IsNullOrWhiteSpace(txtIdProveedor.Text) ||
                    string.IsNullOrWhiteSpace(txtIdDireccion.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                    cmbIdEmpresa.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(txtCiudad.Text) ||
                    string.IsNullOrWhiteSpace(txtColonia.Text) ||
                    string.IsNullOrWhiteSpace(txtCodigoPostal.Text) ||
                    string.IsNullOrWhiteSpace(txtCalle.Text) ||
                    string.IsNullOrWhiteSpace(txtNumeroCasa.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validación de campos numéricos
                if (!int.TryParse(txtIdProveedor.Text, out int idProveedor) ||
                    !int.TryParse(txtIdDireccion.Text, out int idDireccion) ||
                    !int.TryParse(txtTelefono.Text, out int telefono) ||
                    !int.TryParse(txtCodigoPostal.Text, out int codigoPostal) ||
                    !int.TryParse(txtNumeroCasa.Text, out int numeroCasa))
                {
                    MessageBox.Show("Por favor, ingrese valores numéricos válidos en ID, Teléfono, Código Postal y Número de Casa.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Crear objeto Dirección
                Direccion direccion = new Direccion
                {
                    id = idDireccion,
                    ciudad = txtCiudad.Text,
                    colonia = txtColonia.Text,
                    codigoPostal = codigoPostal.ToString(),
                    calle = txtCalle.Text,
                    numeroCasa = numeroCasa.ToString(),
                };

                // Crear objeto Proveedor
                Proveedor proveedor = new Proveedor
                {
                    id = idProveedor,
                    nombre = txtNombre.Text,
                    telefono = telefono.ToString(),
                    estatus = chkEstatus.IsChecked ?? false,
                    idEmpresa = (int)cmbIdEmpresa.SelectedValue,
                    idDireccion = idDireccion,
                };

                // Guardar proveedor y dirección
                _proveedorLogic.GuardarProveedorYDireccion(proveedor, direccion);

                LimpiarControles();
                MessageBox.Show("Operación exitosa.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;

                MessageBox.Show("Error detallado:\n" + inner.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarSiguienteIdProveedor()
        {
            try{
                int siguienteId = _proveedorService.ObtenerSigienteIdProveedor();
                txtIdProveedor.Text = siguienteId.ToString();
            }
            catch (Exception ex){
                MessageBox.Show("Error al obtener el siguiente ID de proveedor: " + ex.Message);
            }
        }
        private void CargarSiguienteIdDireccion(){
            try{
                int siguienteIdD = _direccionService.ObtenerSigienteIdDireccion();
                txtIdDireccion.Text = siguienteIdD.ToString();
            }
            catch (Exception ex){
                MessageBox.Show("Error al obtener el siguiente ID de dirección: " + ex.Message);
            }
        }
        private void LimpiarControles()
        {
            // Limpiar campos del proveedor
            txtIdProveedor.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            chkEstatus.IsChecked = false;
            cmbIdEmpresa.SelectedIndex = -1;

            // Limpiar campos de dirección
            txtCiudad.Clear();
            txtColonia.Clear();
            txtCodigoPostal.Clear();
            txtCalle.Clear();
            txtNumeroCasa.Clear();
            CargarSiguienteIdProveedor();
            CargarSiguienteIdDireccion();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e){
            try{
                if (int.TryParse(txtIdProveedor.Text, out int id)){
                    _proveedorService.EliminarProveedor(id);
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
                var proveedores = _proveedorService.ObtenerProveedores(); 

                string[] columnas = { "id", "nombre", "telefono", "estatus", "idEmpresa","idDireccion" };

                var ventana = new FrmBusqueda("Búsqueda de Clientes", proveedores, columnas);

                if (ventana.ShowDialog() == true) {
                    if (ventana.seleccionado is Proveedor proveedorSeleccionado){
                        txtIdProveedor.Text = proveedorSeleccionado.id.ToString();
                        txtNombre.Text = proveedorSeleccionado.nombre;
                        txtTelefono.Text = proveedorSeleccionado.telefono;
                        chkEstatus.IsChecked = proveedorSeleccionado.estatus;
                        cmbIdEmpresa.SelectedValue = proveedorSeleccionado.idEmpresa;
                        txtIdDireccion.Text = proveedorSeleccionado.idDireccion.ToString();
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
            }
        }
        private void CargarEmpresas(){
            try{
                var empresas = _empresaService.ObtenerEmpresas();

                if (empresas == null || empresas.Count == 0){
                    MessageBox.Show("No se encontraron empresas.");
                    return;
                }
                cmbIdEmpresa.ItemsSource = empresas;
                cmbIdEmpresa.SelectedValuePath = "id";             
                cmbIdEmpresa.DisplayMemberPath = "nombre";

                cmbIdEmpresa.SelectedIndex = -1;
            }
            catch (Exception ex){
                MessageBox.Show("Error al cargar empresas: " + ex.Message);
            }
        }

        private void cmbIdEmpresa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                                                                            
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarControles();
        }
    }
}
