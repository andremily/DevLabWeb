namespace DevLabWebApi.Models
{
    public class Facturas
    {
        public int Id { get; set; }
        public DateTime FechaEmisionFacturas  { get; set; }
        public int IdCliente { get; set; }
        public int NumeroFactura { get; set; }
        public int NumeroTotalArticulos { get; set; }
        public decimal SubTotalFactura { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalFactura { get; set; }
    }

    public class DetalleFacturas
    {
        public int Id { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public int CantidadDeProduto { get; set; }
        public decimal PrecioUnitarioProducto { get; set; }
        public decimal SubtotalProducto { get; set; }
        public string Notas { get; set; }

    }
}
