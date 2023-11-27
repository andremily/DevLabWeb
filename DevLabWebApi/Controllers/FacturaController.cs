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
        private readonly IClientes _clientes;
        private readonly IProductos _productos;
        private readonly IFactura _factura;

        public FacturaController(IClientes clientes, IProductos productos, IFactura factura)
        {
            _clientes = clientes;
            _productos = productos;
            _factura = factura;
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

        [HttpPost("GuardarFactura")]
        public Response GuardarFactura(FacturaCompleta factura)
        {
            try
            {
                Response response = _factura.GuardarFactura(factura);
                return response;
            }
            catch(Exception ex)
            {
                return new Response { Resultado = false, Mensaje = ex.Message };
            }
            
        }
    }
}
