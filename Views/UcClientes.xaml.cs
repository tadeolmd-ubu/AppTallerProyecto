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
using AppTaller.Model;
using AppTaller.Services;


namespace AppTaller.Views
{
    /// <summary>
    /// Lógica de interacción para UcClientes.xaml
    /// </summary>
    public partial class UcClientes : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly Logics.ClienteLogic _logic;
        private readonly ClienteService _clienteService;
        private readonly DireccionService _direccionService;
        
        public UcClientes()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _clienteService = new ClienteService(_context);
            _direccionService = new DireccionService(_context);
            _logic = new Logics.ClienteLogic();
        }
       
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
           try {
             
                if (string.IsNullOrWhiteSpace(txtNombreCliente.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                    string.IsNullOrWhiteSpace(txtCiudad.Text) ||
                    string.IsNullOrWhiteSpace(txtColonia.Text) ||
                    string.IsNullOrWhiteSpace(txtCodigoPostal.Text) ||
                    string.IsNullOrWhiteSpace(txtCalle.Text) ||
                    string.IsNullOrWhiteSpace(txtNumeroCasa.Text) ||
                    string.IsNullOrWhiteSpace(txtIdCliente.Text) ||
                    string.IsNullOrWhiteSpace(txtIdDireccion.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtCodigoPostal.Text, out int codigoPostal) ||
                    !int.TryParse(txtNumeroCasa.Text, out int numeroCasa) ||
                    !int.TryParse(txtIdCliente.Text, out int idCliente) ||
                    !int.TryParse(txtIdDireccion.Text, out int idDireccion) ||
                    !int.TryParse(txtTelefono.Text, out int telefono))
                {
                    MessageBox.Show("Por favor, ingrese valores numéricos válidos para Código Postal y Número de Casa.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Direccion direccion = new Direccion
                {
                    id = idDireccion,
                    ciudad = txtCiudad.Text,
                    colonia = txtColonia.Text,
                    codigoPostal = codigoPostal.ToString(),
                    calle = txtCalle.Text,
                    numeroCasa = numeroCasa.ToString(),
                };

                Cliente cliente = new Cliente
                {
                    id =  idCliente,
                    nombre = txtNombreCliente.Text,
                    telefono = telefono.ToString(),
                    estatus = chkEstatus.IsChecked ?? false,
                    idDireccion = idDireccion,
                };                

                
                _logic.GuardarClienteYDireccion(cliente, direccion);

                LimpiarControles();
                MessageBox.Show("Operacion exitosa.");

            } catch(Exception ex) {
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;

                MessageBox.Show("Error detallado:\n" + inner.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LimpiarControles() {
            txtCalle.Text = " ";
            txtTelefono.Text = " ";
            txtCiudad.Text = " ";
            txtCodigoPostal.Text = " ";
            txtColonia.Text = " ";
            txtIdCliente.Text = " ";
            txtIdDireccion.Text = " ";
            txtNombreCliente.Text = " ";
            txtNumeroCasa.Text = " ";
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (int.TryParse(txtIdCliente.Text, out int id)){
                    _clienteService.EliminarCliente(id);
                }
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                MessageBox.Show("Error detallado:\n" + inner.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientes= _clienteService.ObtenerClientes(); // List<Usuario>

                string[] columnas = { "id", "nombre", "telefono", "estatus", "idDireccion"};

                var ventana = new FrmBusqueda("Búsqueda de Clientes", clientes, columnas);

                if (ventana.ShowDialog() == true)
                {
                    if (ventana.seleccionado is Cliente clienteSeleccionado)
                    {
                        txtIdCliente.Text = clienteSeleccionado.id.ToString();
                        txtNombreCliente.Text = clienteSeleccionado.nombre;
                        txtTelefono.Text = clienteSeleccionado.telefono;
                        chkEstatus.IsChecked = clienteSeleccionado.estatus;
                        txtIdDireccion.Text = clienteSeleccionado.idDireccion.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
            }
        }

        private void btnBuscar_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
