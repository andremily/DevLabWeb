
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;

namespace DevLabFront.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InicioController : Controller
    {
        public IConfiguration _configuration { get; set; }
       public HttpClient httpClient { get; set; } = new HttpClient();
        public InicioController(IConfiguration configuration) {
        
        _configuration = configuration;
        }

        [HttpGet ("ObtenerUrl")]
        public string ObtenerUrl()
        {
            var url = _configuration.GetSection("UrlApi").Value;
            return url;
        }

        [HttpPost("ObtenerClientesAsync")]
        public async Task<List<ClientesModel>> ObtenerClientesAsync()
        {
            string responseClientes;

            try
            {

                var baseAddressApi = _configuration.GetSection("UrlApi").Value;
                var request = JsonContent.Create(string.Empty);
                var response = await httpClient.PostAsync($"{baseAddressApi}Factura/ConsultarClientes", request);
                if (response.IsSuccessStatusCode)
                {
                    responseClientes = await response.Content.ReadAsStringAsync();
                    
                    return JsonSerializer.Deserialize<List<ClientesModel>>(responseClientes)!; 
                }
                else
                {
                    responseClientes = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                responseClientes = $"Error: {ex.Message}";
            }
            return new();
        }
        [HttpPost("ObtenerProductosAsync")]
        public async Task<List<ProductosModel>> ObtenerProductosAsync()
        {
            string responseProductos;

            try
            {

                var baseAddressApi = _configuration.GetSection("UrlApi").Value;
                var request = JsonContent.Create(string.Empty);
                var response = await httpClient.PostAsync($"{baseAddressApi}Factura/ConsultarProductos", request);
                if (response.IsSuccessStatusCode)
                {
                    responseProductos = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<List<ProductosModel>>(responseProductos)!;
                }
                else
                {
                    responseProductos = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                responseProductos = $"Error: {ex.Message}";
            }
            return new();
        }
        [HttpPost("GuardarFacturaAsync")]
        public async Task<Response> GuardarFacturaAsync(FacturaCompleta factura)
        {
            string responseFactura;

            try
            {

                var baseAddressApi = _configuration.GetSection("UrlApi").Value;
                var request = JsonContent.Create(factura);
                var response = await httpClient.PostAsync($"{baseAddressApi}Factura/GuardarFactura", request);
                if (response.IsSuccessStatusCode)
                {
                    responseFactura = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<Response>(responseFactura)!;
                }
                else
                {
                    responseFactura = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return new Response { Resultado = false, Mensaje = responseFactura };
                }
            }
            catch (HttpRequestException ex)
            {
                return new Response { Resultado = false, Mensaje = ex.Message };
            }
            
        }
    }
}
