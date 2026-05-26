using System.ComponentModel.DataAnnotations;

namespace AppTaller.Model
{
    public class vw_ProductosMasVendidos
    {
        public int idProducto { get; set; }
        public string producto { get; set; }
        public string marca { get; set; }
        public string tipoProducto { get; set; }
        public int totalVendido { get; set; }
        public int vecesVendido { get; set; }
        public decimal ingresoGenerado { get; set; }
    }
}
