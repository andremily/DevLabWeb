using DevLabWebApi.Models;
using Models;

namespace DevLabWebApi.Services
{
    public interface IClientes
    {
        public List<ClientesModel> ObtenerConsultaClienteLista();
    }
}
