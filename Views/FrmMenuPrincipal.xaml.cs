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
using System.Runtime;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace AppTaller.Views
{
    /// <summary>
    /// Lógica de interacción para FrmMenuPrincipal.xaml
    /// </summary>
    public partial class FrmMenuPrincipal : Window
    {
        
        private UcUsuario _ucUsuario;
        private UcInicio _ucInicio;
        private UcClientes _ucClientes;
        private UcProveedores _ucProveedores;
        private UcProductos _ucProductos;
        public FrmMenuPrincipal()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void pnlDeControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, (IntPtr)2, IntPtr.Zero);
        }

        private void pnDeControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }





        //metodo para mostrar los forms dentro del mismo menu 

        private void inicio_Checked(object sender, RoutedEventArgs e)
        {
            if (_ucInicio == null)
            {
                _ucInicio = new UcInicio();
            }

            // 2. Asignar la instancia al ContentControl en el XAML
            // Esto hace que el UserControl se muestre dentro de tu FrmMenuPrincipal
            contenedorFormularios.Content = _ucInicio;
        }

        private void Usuario_Checked(object sender, RoutedEventArgs e)
        {
            // 1. Instanciar el UserControl (o reutilizar si ya existe)
            if (_ucUsuario == null) {
                _ucUsuario = new UcUsuario();
            }

            // 2. Asignar la instancia al ContentControl en el XAML
            // Esto hace que el UserControl se muestre dentro de tu FrmMenuPrincipal
            contenedorFormularios.Content = _ucUsuario;

        }
         
        private void Clientes_Checked(object sender, RoutedEventArgs e)
        {
            if (_ucClientes == null) {
            _ucClientes = new UcClientes();
            }

            contenedorFormularios.Content = _ucClientes;
        }

        private void Proveedores_Checked(object sender, RoutedEventArgs e)
        {
            if (_ucProveedores == null){
                _ucProveedores = new UcProveedores();
            }

            contenedorFormularios.Content = _ucProveedores;

        }

        private void Productos_Checked(object sender, RoutedEventArgs e)
        {
            if (_ucProductos == null) {
                _ucProductos = new UcProductos();
            }
            contenedorFormularios.Content = _ucProductos;
        }
    }
}
