using DevLabWebApi.Models;
using Microsoft.AspNetCore.Components;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DevLabWebApi.Services
{
    public class Productos : IProductos
    {
        [Inject] public IConfiguration? Configuration { get; set; }
        public Productos(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<ProductosModel> ObtenerProductos()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                List<ProductosModel> productos = new List<ProductosModel>();

                using (SqlCommand cmd = new SqlCommand("ConsultarProductos", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductosModel producto = new ProductosModel
                            {
                                Id = reader.GetInt32(0),
                                NombreProducto = reader.GetString(1),
                                PrecioUnitario = reader.GetDecimal(2),
                                ext = reader.GetString(3),
                                ImagenProducto = reader.IsDBNull(4) ? null : reader.GetString(4),
                            };

                            productos.Add(producto);
                        }
                    }
                }
                return productos;
               
            }
        }
    }
}
