using DevLabWebApi.Models;
using Microsoft.AspNetCore.Components;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace DevLabWebApi.Services
{
    public class Clientes : IClientes
    {
        [Inject] public IConfiguration? Configuration { get; set; }
        public Clientes(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public List<ClientesModel> ObtenerConsultaClienteLista()
        {
            try
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    List<ClientesModel> clientes = new List<ClientesModel>();

                    using (SqlCommand cmd = new SqlCommand("ConsultaClientes", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientesModel cliente = new ClientesModel
                                {
                                    Id = reader.GetInt32(0),           
                                    RazonSocial = reader.GetString(1)  
                                };

                                clientes.Add(cliente);
                            }
                        }
                    }
                    return clientes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ClientesModel>();
            }
           
            
        }
    }
}
