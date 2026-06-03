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
    /// Lógica de interacción para UcUsuario.xaml
    /// </summary>
    public partial class UcUsuario : UserControl
    {
        private readonly EF.efAppDbContext _context;
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;

        public UcUsuario()
        {
            InitializeComponent();
            _context = new EF.efAppDbContext();
            _usuarioService = new UsuarioService(_context);
            _rolService = new RolService(_context);

            CargarRoles();
            LimpiarCampos();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {               
                try{

                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                    string.IsNullOrWhiteSpace(txtContrasena.Password) ||
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                    string.IsNullOrWhiteSpace(cmbIdRol.Text)) {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtId.Text, out int id)) {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido para el ID del usuario.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (cmbIdRol.SelectedValue == null || !int.TryParse(cmbIdRol.SelectedValue.ToString(), out int idRol)) {
                    MessageBox.Show("Por favor, seleccione un rol válido.", "Entrada inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                    // Crear objeto usuario con los datos del formulario
                    Usuario usuario = new Usuario {
                        id = id,
                        nombre = txtNombre.Text,
                        correo = txtCorreo.Text,
                        contrasena = txtContrasena.Password,
                        telefono = txtTelefono.Text,
                        estatus = estatus.IsChecked ?? false,
                        idRol = idRol
                    };

                    _usuarioService.CrearOActualizarUsuario(usuario);

                    LimpiarCampos();

                    MessageBox.Show("Operación exitosa");
                }
                catch (Exception ex) {
                    MessageBox.Show("Error al guardar o modificar: " + ex.Message);
                }
            
        }    
        private void CargarRoles() {
            try{
                var roles = _rolService.ObtenerRoles();

                if (roles == null || roles.Count == 0){
                    MessageBox.Show("No se encontraron los roles.");
                    return;
                }
                cmbIdRol.ItemsSource = roles;
                cmbIdRol.SelectedValuePath = "id";
                cmbIdRol.DisplayMemberPath = "nombre";

                cmbIdRol.SelectedIndex = -1;
            }
            catch (Exception ex){
                MessageBox.Show("Error al cargar roles: " + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try {
                var usuarios = _usuarioService.ObtenerUsuarios();

                string[] columnas = { "id", "nombre", "correo", "telefono", "estatus", "idRol" };

                var ventana = new FrmBusqueda("Búsqueda de Usuarios", usuarios, columnas);

                if (ventana.ShowDialog() == true) {
                    if (ventana.seleccionado is Usuario usuarioSeleccionado) {
                        txtId.Text = usuarioSeleccionado.id.ToString();
                        txtNombre.Text = usuarioSeleccionado.nombre;
                        txtCorreo.Text = usuarioSeleccionado.correo;
                        txtTelefono.Text = usuarioSeleccionado.telefono;
                        estatus.IsChecked = usuarioSeleccionado.estatus;
                        cmbIdRol.SelectedValue = usuarioSeleccionado.idRol;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
            }
        }
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (int.TryParse(txtId.Text, out int id)) {
                    _usuarioService.EliminarUsuario(id);

                    MessageBox.Show("Usuario borrado exitosamente");
                    LimpiarCampos();
                }
                else {
                    MessageBox.Show("ID inválido");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error al borrar: " + ex.Message);
            }
        }
        private void LimpiarCampos() {
            txtId.Text = _usuarioService.ObtenerSigienteIdUsuario().ToString();
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtContrasena.Password= "";
            estatus.IsChecked = false;
            cmbIdRol.SelectedIndex = -1;
            txtId.Focus();

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
