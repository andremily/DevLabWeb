using DevLabWebApi.Models;
using DevLabWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace DevLabWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturaController : Controller
    {
        public IClientes _clientes { get; set; }
        public IProductos _productos { get; set; }

        public FacturaController(IClientes clientes, IProductos productos)
        {
            _clientes = clientes;
            _productos = productos;
        }

        [HttpPost ("ConsultarClientes")]
        public List<ClientesModel> ConsultarClientes()
        {
            List<ClientesModel> clientes = _clientes.ObtenerConsultaClienteLista();
            return clientes;
        }
        [HttpPost("ConsultarProductos")]
        public List<ProductosModel> ConsultarProductos()
        {
            List<ProductosModel> productos = _productos.ObtenerProductos();
            return productos;
        }


    }
}
