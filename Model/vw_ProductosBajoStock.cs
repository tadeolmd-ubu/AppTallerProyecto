using System.ComponentModel.DataAnnotations;

namespace AppTaller.Model
{
    public class vw_ProductosBajoStock
    {
        public int idInventario { get; set; }
        public int idProducto { get; set; }
        public string producto { get; set; }
        public string marca { get; set; }
        public string tipoProducto { get; set; }
        public int idAlmacen { get; set; }
        public string almacen { get; set; }
        public int stockActual { get; set; }
        public int stockMinimoSugerido { get; set; }
        public string prioridad { get; set; }
    }
}
