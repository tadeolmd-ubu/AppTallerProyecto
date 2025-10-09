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
        public UcClientes()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtIdCliente.Text, out int id);

            Cliente cliente = new Cliente() {
            
             id = id
             
            
            
            };






            try {

                ClienteService service = new ClienteService();

                service.CrearOActualizarCliente(cliente);
                MessageBox.Show("Operacion exitosa");

            }
            catch (Exception ex){
                MessageBox.Show("Error: " + ex.Message);
            }



        }
    }
}
