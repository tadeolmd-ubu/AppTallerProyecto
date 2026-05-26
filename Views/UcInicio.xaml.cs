using AppTaller.EF;
using AppTaller.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Controls;

namespace AppTaller.Views
{
    public partial class UcInicio : UserControl
    {
        private readonly efAppDbContext _context;

        public UcInicio()
        {
            InitializeComponent();
            _context = new efAppDbContext();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Session.UsuarioActual != null)
                lblBienvenida.Text = $"Bienvenido, {Session.UsuarioActual.nombre}";

            CargarDashboard();
            CargarTopProductos();
            CargarVentasDiarias();
        }

        private void CargarDashboard()
        {
            try
            {
                using (var conn = new SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["DbTaller"].ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("sp_ObtenerDashboard", conn)
                    { CommandType = CommandType.StoredProcedure })
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var indicador = reader["indicador"].ToString();
                                var valor = reader["valor"];

                                switch (indicador)
                                {
                                    case "ProductosActivos":
                                        lblProductos.Text = valor.ToString();
                                        break;
                                    case "ClientesActivos":
                                        lblClientes.Text = valor.ToString();
                                        break;
                                    case "VentasHoy":
                                        lblVentasHoy.Text = valor.ToString();
                                        break;
                                    case "TotalVentasHoy":
                                        lblTotalVentas.Text = string.Format("{0:C}", valor);
                                        break;
                                    case "PresupuestosActivos":
                                        lblPresupuestos.Text = valor.ToString();
                                        break;
                                    case "ProductosBajoStock":
                                        lblBajoStock.Text = valor.ToString();
                                        break;
                                    case "UsuariosActivos":
                                        lblUsuarios.Text = valor.ToString();
                                        break;
                                    case "ProveedoresActivos":
                                        lblProveedores.Text = valor.ToString();
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar dashboard: {ex.Message}");
            }
        }

        private void CargarTopProductos()
        {
            try
            {
                var productos = _context.ProductosMasVendidos
                    .OrderByDescending(p => p.totalVendido)
                    .Take(10)
                    .ToList();
                dgProductosTop.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar top productos: {ex.Message}");
            }
        }

        private void CargarVentasDiarias()
        {
            try
            {
                var ventas = _context.ResumenVentasPorDia
                    .OrderByDescending(v => v.fecha)
                    .Take(15)
                    .ToList();
                dgVentasDiarias.ItemsSource = ventas;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al cargar ventas diarias: {ex.Message}");
            }
        }
    }
}
