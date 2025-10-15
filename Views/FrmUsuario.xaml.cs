using AppTaller.EF;
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
using System.Windows.Shapes;

namespace AppTaller.Views
{
    /// <summary>
    /// Lógica de interacción para FrmUsuario.xaml
    /// </summary>
    public partial class FrmUsuario : Window
    {
        bool existe;
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;
        // DI inyecta automáticamente el servicio
        public FrmUsuario()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarRoles();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Crear objeto usuario con los datos del formulario
                Usuario usuario = new Usuario
                {
                    id = int.Parse(txtId.Text),
                    nombre = txtNombre.Text,
                    correo = txtCorreo.Text,
                    contrasena = txtContrasena.Password,
                    telefono = txtTelefono.Text,
                    estatus = estatus.IsChecked ?? false,
                    idRol = int.Parse(idRol.Text)
                };
                _usuarioService.CrearOActualizarUsuario(usuario);

                LimpiarCampos();

                MessageBox.Show("Operacion exitosa");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar o modificar: " + ex.Message);
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
{
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void CargarRoles()
        {
            
            var roles = _rolService.ObtenerRoles();

            idRol.ItemsSource = roles;
            if (roles.Count > 0)
                idRol.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            
            var usuarios = _usuarioService.ObtenerUsuarios(); // List<Usuario>

            // Los nombres deben coincidir EXACTAMENTE con las propiedades de Usuario
            string[] columnas = { "id", "nombre", "correo", "telefono", "estatus", "idRol" };

            var ventana = new FrmBusqueda("Búsqueda de Usuarios", usuarios, columnas);

            if (ventana.ShowDialog() == true)
            {
                var usuarioSeleccionado = ventana.seleccionado as Usuario;
                if (usuarioSeleccionado != null)
                {
                    txtId.Text = usuarioSeleccionado.id.ToString();
                    txtNombre.Text = usuarioSeleccionado.nombre;
                    txtCorreo.Text = usuarioSeleccionado.correo;
                    txtTelefono.Text = usuarioSeleccionado.telefono;

                }
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _usuarioService.EliminarUsuario(int.Parse(txtId.Text));

                MessageBox.Show("Usuario borrado exitosamente");
                LimpiarCampos();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al borrar: " + ex.Message);
            }
        }

        // Método de instancia
        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtContrasena.Password = "";
            estatus.IsChecked = false;
            idRol.SelectedIndex = -1;
            txtId.Focus();
        }

    }
}
