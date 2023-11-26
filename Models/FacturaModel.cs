using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FacturaModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int NumeroFactura { get; set; }
        public int NumeroTotalArticulos { get; set; }
        public decimal SubTotalFactura { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalFactura { get; set; }
    }

    public class DetalleFacturaModel
    {
        public int Id { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public int CantidadDeProducto { get; set; }
        public decimal PrecioUnitarioProducto { get; set; }
        public decimal SubtotalProducto { get; set; }
        public string Notas { get; set; } = String.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string Imagen { get; set; }
    }
}
