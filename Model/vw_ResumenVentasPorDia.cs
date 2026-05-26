using System;
using System.ComponentModel.DataAnnotations;

namespace AppTaller.Model
{
    public class vw_ResumenVentasPorDia
    {
        public DateTime fecha { get; set; }
        public int cantidadVentas { get; set; }
        public decimal totalVentas { get; set; }
        public decimal promedioPorVenta { get; set; }
        public decimal ventaMaxima { get; set; }
        public int clientesAtendidos { get; set; }
    }
}
