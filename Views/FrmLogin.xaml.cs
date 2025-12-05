using AppTaller.Model;
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
    /// Lógica de interacción para FrmLogin.xaml
    /// </summary>
    public partial class FrmLogin : Window
    {
       
        private readonly EF.efAppDbContext _context;

        public FrmLogin()
        {
            _context = new EF.efAppDbContext();
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        int x = 3;
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {



            int.TryParse(txtUsuario.Text, out int id);
            string password = txtContrasena.Password;

            Login login = new Login();

            if (login.ValidarUsuario(id, password))
            {
                var usuario = _context.Usuario.FirstOrDefault(u => u.id == id);

                Session.UsuarioActual = usuario;

                FrmMenuPrincipal mp = new FrmMenuPrincipal();
                mp.Show();
                this.Hide();
            }
            else
            {
                x--;
                MessageBox.Show($"Usuario o contraseña incorrectos, le quedan {x} intentos.");
            }

            if (x == 0)
            {
                txtContrasena.IsEnabled = false;
                txtUsuario.IsEnabled = false;
                btnLogin.IsEnabled = false;
                MessageBox.Show("Límite de intentos alcanzados.");
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
