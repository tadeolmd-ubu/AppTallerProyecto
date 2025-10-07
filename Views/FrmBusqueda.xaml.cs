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
    /// Lógica de interacción para FrmBusqueda.xaml
    /// </summary>
    public partial class FrmBusqueda : Window
    {


        public object seleccionado { get; private set; }
        public FrmBusqueda(string titulo, IEnumerable<object> datos, string[] columnas)
        {
            InitializeComponent();

            this.Title = titulo;

            // Limpiar columnas existentes
            dataGridBusqueda.Columns.Clear();

            foreach (var col in columnas)
            {
                dataGridBusqueda.Columns.Add(new DataGridTextColumn
                {
                    Header = col,
                    Binding = new Binding(col) // debe coincidir exactamente con el nombre de la propiedad
                });
            }

            // Asignar los datos al DataGrid
            dataGridBusqueda.ItemsSource = datos;

            dataGridBusqueda.MouseDoubleClick += (s, e) =>
            {
                SeleccionarFila();
            };
        }
        private void SeleccionarFila()
        {
            if (dataGridBusqueda.SelectedItem != null)
            {
                seleccionado = dataGridBusqueda.SelectedItem;
                this.DialogResult = true; // cierra la ventana
                this.Close();
            }
        }
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
