using DevLabWebApi.Models;
using Models;

namespace DevLabWebApi.Services
{
    public interface IProductos
    {
        public List<ProductosModel> ObtenerProductos();
    }
}
