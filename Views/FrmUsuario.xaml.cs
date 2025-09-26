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
                var usuario = new Usuario
                {
                    id = 1004,
                    nombre = txtNombre.Text,
                    correo = txtCorreo.Text,
                    contrasena = txtContrasena.Password,
                    telefono = txtTelefono.Text,
                    estatus = estatus.IsChecked ?? false,
                    idRol = int.Parse(idRol.Text)
                };

                using (var context = new efAppDbContext())
                {
                    context.Usuario.Add(usuario);
                    context.SaveChanges();
                }

                MessageBox.Show("Usuario guardado con éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
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
            RolService servicio = new RolService();
            var roles = servicio.ObtenerRoles();

            idRol.ItemsSource = roles;
            if (roles.Count > 0)
                idRol.SelectedIndex = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
