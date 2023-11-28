using Microsoft.AspNetCore.Components;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DevLabWebApi.Services
{
    public class Factura : IFactura
    {
        [Inject] public IConfiguration? Configuration { get; set; }
        public Factura(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<FacturaModel> GetFacturaList(int? numeroFactura, int? idCliente)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                List<FacturaModel> facturas = new List<FacturaModel>();

                using (SqlCommand cmd = new SqlCommand("ConsultaNumeroFactura", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NumeroFactura", SqlDbType.Int) { Value = numeroFactura.HasValue ? (object)numeroFactura.Value : DBNull.Value });
                        cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = idCliente.HasValue ? (object)idCliente.Value : DBNull.Value });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FacturaModel factura = new FacturaModel
                            {
                                NumeroFactura = reader.GetInt32(0),
                                FechaEmisionFactura = reader.GetDateTime(1),
                                TotalFactura = reader.GetDecimal(2),
                                IdCliente = reader.GetInt32(3)
                            };

                            facturas.Add(factura);
                        }
                    }
                }
                return facturas;

            }
        }

        public Response GuardarFactura(FacturaCompleta factura)
        {
            List<FacturaModel> responseFactura = GetFacturaList(factura.Factura.NumeroFactura, null);
            if(responseFactura is not null &&  responseFactura.Count > 0)
            {
                return new Response
                {
                    Resultado = false,
                    Mensaje = "El número de la factura ya se encuentra registrado"
                };
            }
            return GuardarFacturaDB(factura);
        }
        private Response GuardarFacturaDB(FacturaCompleta facturaCompleta)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    object idFactura = 0;
                    using (SqlCommand cmd = new SqlCommand("GuardarFactura", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FechaEmisionFactura", SqlDbType.DateTime) { Value = facturaCompleta.Factura.FechaEmisionFactura });
                        cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = facturaCompleta.Factura.IdCliente });
                        cmd.Parameters.Add(new SqlParameter("@NumeroFactura", SqlDbType.Int) { Value = facturaCompleta.Factura.NumeroFactura });
                        cmd.Parameters.Add(new SqlParameter("@NumeroTotalArticulos", SqlDbType.Int) { Value = facturaCompleta.Factura.NumeroTotalArticulos });
                        cmd.Parameters.Add(new SqlParameter("@SubTotalFactura", SqlDbType.Decimal) { Value = facturaCompleta.Factura.SubTotalFactura });
                        cmd.Parameters.Add(new SqlParameter("@TotalImpuesto", SqlDbType.Decimal) { Value = facturaCompleta.Factura.TotalImpuesto });
                        cmd.Parameters.Add(new SqlParameter("@TotalFactura", SqlDbType.Decimal) { Value = facturaCompleta.Factura.TotalFactura });
                        idFactura = cmd.ExecuteScalar(); 
                    }

                    using (SqlCommand cmd = new SqlCommand("GuardarDetalleFactura", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var item in facturaCompleta.Detalle)
                        {
                            cmd.Parameters.Clear(); 

                            cmd.Parameters.Add(new SqlParameter("@IdFactura", SqlDbType.Int) { Value = idFactura });
                            cmd.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.Int) { Value = item.IdProducto });
                            cmd.Parameters.Add(new SqlParameter("@CantidadDeProducto", SqlDbType.Int) { Value = item.CantidadDeProducto });
                            cmd.Parameters.Add(new SqlParameter("@PrecioUnitarioProducto", SqlDbType.Decimal) { Value = item.PrecioUnitarioProducto });
                            cmd.Parameters.Add(new SqlParameter("@SubtotalProducto", SqlDbType.Decimal) { Value = item.SubtotalProducto });
                            cmd.Parameters.Add(new SqlParameter("@Notas", SqlDbType.VarChar) { Value = item.Notas });
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return new Response { Resultado = true, Mensaje = "Su factura fue creada exitosamente." };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response { Resultado = false, Mensaje = ex.Message };
                }
            }

         

        }
    }
}