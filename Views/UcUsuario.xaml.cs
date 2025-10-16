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
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {                // Crear objeto usuario con los datos del formulario
                try{
                    // Crear objeto usuario con los datos del formulario
                    Usuario usuario = new Usuario {
                        id = int.TryParse(txtId.Text, out int idValue) ? idValue : 0,
                        nombre = txtNombre.Text,
                        correo = txtCorreo.Text,
                        contrasena = txtContrasena.Password,
                        telefono = txtTelefono.Text,
                        estatus = estatus.IsChecked ?? false,
                        idRol = int.TryParse(idRol.Text, out int rolValue) ? rolValue : 0
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

                idRol.ItemsSource = roles;
                idRol.SelectedValuePath = "id";     

                if (roles.Count > 0)
                    idRol.SelectedIndex = 0;
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
                var usuarios = _usuarioService.ObtenerUsuarios(); // List<Usuario>

                string[] columnas = { "id", "nombre", "correo", "telefono", "estatus", "idRol" };

                var ventana = new FrmBusqueda("Búsqueda de Usuarios", usuarios, columnas);

                if (ventana.ShowDialog() == true) {
                    if (ventana.seleccionado is Usuario usuarioSeleccionado) {
                        txtId.Text = usuarioSeleccionado.id.ToString();
                        txtNombre.Text = usuarioSeleccionado.nombre;
                        txtCorreo.Text = usuarioSeleccionado.correo;
                        txtTelefono.Text = usuarioSeleccionado.telefono;
                        estatus.IsChecked = usuarioSeleccionado.estatus;
                        idRol.SelectedValue = usuarioSeleccionado.idRol;
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
        // Método de instancia
        private void LimpiarCampos() {
            txtId.Text = "";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtContrasena.Password= "";
            estatus.IsChecked = false;
            idRol.SelectedIndex = -1;
            txtId.Focus();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
