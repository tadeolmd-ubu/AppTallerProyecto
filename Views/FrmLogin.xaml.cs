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
        public FrmLogin()
        {
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

           

            //lo que va a ingresar el usuario
            int.TryParse(txtUsuario.Text, out int id);

            string password = txtContrasena.Password;
          

                //se crea una instania de login para poder usar el metodo de validar el usuario
                Login login = new Login();
                //se valida si existe
                if (login.ValidarUsuario(id, password))
                {
                 
                    //en caso de existi, muestra el menu y oculta el login
                    FrmMenuPrincipal mp = new FrmMenuPrincipal();
                   mp.Show();


                
                

                    this.Hide();
                }
                else
                {
                    //sino pos dice que es incorrecto algo y se le resta uno al limite de intentos
                    x--;
                    MessageBox.Show($"Usuario o contraseña incorrectos, le quedan {x} intentos.");
                    
                }
           

            if (x == 0) { 
                txtContrasena.IsEnabled = false;
                txtUsuario.IsEnabled = false;
                btnLogin.IsEnabled = false;
                MessageBox.Show("Limite de intentos alcanzados.");
            }

            //FrmMenuPrincipal menu = new FrmMenuPrincipal();
            //menu.Show();

            //this.Hide();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
