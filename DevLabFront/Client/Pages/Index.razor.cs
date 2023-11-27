using Microsoft.AspNetCore.Components;
using Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevLabFront.Client.Pages
{
    public partial class Index
    {
        public bool AdicionProductos { get; set; } = false;
        public FacturaModel Factura { get; set; } = new();
        public string? BaseAddressApi { get; set; }
        public List<ClientesModel> ListaClientes { get; set; } = new();
        public List<ProductosModel> ListaProductos { get; set; } = new();
        public List<DetalleFacturaModel> DetalleFactura { get; set; } = new();

        private DetalleFacturaModel Detalle = new DetalleFacturaModel();
        [Inject] public HttpClient httpClient { get; set; } = new HttpClient();
        public List<string> Mensaje { get; set; } = new();
        public bool AbrirMensaje { get; set; } = false;
        public string? Titulo { get; set; }
        protected override async Task OnInitializedAsync()
        {
            BaseAddressApi = await httpClient.GetStringAsync("Inicio/ObtenerUrl");
            await ObtenerClientes();

        }
        private async Task ObtenerClientes()
        {
            try
            {
                var request = JsonContent.Create(string.Empty);
                var response = await httpClient.PostAsync("Inicio/ObtenerClientesAsync", request);
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadAsStringAsync();
                    ListaClientes = JsonSerializer.Deserialize<List<ClientesModel>>(lista)!;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task ObtenerProductos()
        {
            try
            {
                var request = JsonContent.Create(string.Empty);
                var response = await httpClient.PostAsync("Inicio/ObtenerProductosAsync", request);
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadAsStringAsync();
                    ListaProductos = JsonSerializer.Deserialize<List<ProductosModel>>(lista)!;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AdicionarNuevaFactura()
        {
            Factura = new FacturaModel();
            DetalleFactura = new();
            Detalle = new();
        }
        public async Task GuardarFacturaAsync(){
           bool valido = ValidarCampos();
            if (valido)
            {
                Factura.FechaEmisionFactura = DateTime.Now;
                FacturaCompleta factura = new FacturaCompleta
                {
                    Factura = Factura,
                    Detalle = DetalleFactura
                };

                try
                {
                    var request = JsonContent.Create(factura);
                    var response = await httpClient.PostAsync("Inicio/GuardarFacturaAsync", request);
                    if (response.IsSuccessStatusCode)
                    {
                        var stringResponse = await response.Content.ReadAsStringAsync();
                        var respuesta = JsonSerializer.Deserialize<Response>(stringResponse)!;
                        ManejoMensajes(respuesta);
                    }
                    else
                    {
                        
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                AbrirMensaje = true;
                StateHasChanged();
            }
        }

        private void ManejoMensajes(Response respuesta)
        {
            if (respuesta.Resultado)
            {
                Titulo = "Operación Exitosa";
                Mensaje.Add(respuesta.Mensaje);
                AbrirMensaje = true;
                Factura = new();
                Detalle = new();
                DetalleFactura = new();
            }
            else
            {
                Titulo = "Error";
                Mensaje.Add(respuesta.Mensaje);
                AbrirMensaje = true;
            }
        }

        private bool ValidarCampos()
        {
            Titulo = "Error";
            var valido = true;
            Mensaje = new();
           if (Factura.IdCliente == 0)
            {
                Mensaje.Add("Debe seleccionar un cliente.");
                valido = false;
            }
            if (Factura.NumeroFactura == 0)
            {
                Mensaje.Add("Debe digitar número de factura");
                valido = false;
            }
            if (DetalleFactura.Count == 0)
            {
                Mensaje.Add("Debe adicionar los productos");
                valido = false;
            }
            return valido;
        }
        public async Task AdicionarProductosAsync()
        {
            await ObtenerProductos();
            Detalle = new();
            AdicionProductos = true;
            StateHasChanged();

        }
        public void Eliminar(DetalleFacturaModel detalle)
        {
            Factura.SubTotalFactura -= Detalle.SubtotalProducto;
            Factura.TotalImpuesto = (Factura.SubTotalFactura * 19) / 100;
            Factura.TotalFactura = Factura.SubTotalFactura + Factura.TotalImpuesto;
            Factura.NumeroTotalArticulos -= Detalle.CantidadDeProducto;
            DetalleFactura.Remove(detalle);

     
        }

        public void GuardarProducto()
        {
            ProductosModel productoSeleccionado = ListaProductos.FirstOrDefault(x=>x.Id== Detalle.IdProducto)!;
            Detalle.Imagen = productoSeleccionado.ImagenProducto!;
            Detalle.NombreProducto = productoSeleccionado.NombreProducto!;
            Detalle.PrecioUnitarioProducto = productoSeleccionado.PrecioUnitario;
            Detalle.SubtotalProducto = productoSeleccionado.PrecioUnitario * Detalle.CantidadDeProducto;
            Factura.SubTotalFactura += Detalle.SubtotalProducto;
            Factura.TotalImpuesto = (Factura.SubTotalFactura * 19) / 100;
            Factura.TotalFactura = Factura.SubTotalFactura + Factura.TotalImpuesto;
            Factura.NumeroTotalArticulos += Detalle.CantidadDeProducto;
            DetalleFactura.Add(Detalle);
            AdicionProductos = false;
            StateHasChanged();
        }
    }
}
