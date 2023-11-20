using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductosModel
    {
        public int Id { get; set; }
        public string? NombreProducto { get; set; }
        public byte[]? ImagenProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? ext { get; set; }
    }
}
