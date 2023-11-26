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
        public decimal SubTotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public List<string> Mensaje { get; set; }
        public bool AbrirMensaje { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            BaseAddressApi = await Http.GetStringAsync("Inicio/ObtenerUrl");
            await ObtenerClientes();

        }

        private async Task ObtenerClientes()
        {
            try
            {
                var request = JsonContent.Create(string.Empty);
                var response = await Http.PostAsync("Inicio/ObtenerClientesAsync", request);
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
                var response = await Http.PostAsync("Inicio/ObtenerProductosAsync", request);
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
            ListaProductos = new List<ProductosModel>();
        }
        public void GuardarFactura(){
           bool valido = ValidarCampos();
            if (valido)
            {
                //enviar a guardar
            }
            else
            {
                AbrirMensaje = true;
                StateHasChanged();
            }
        }

        private bool ValidarCampos()
        {
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
            SubTotal -= Detalle.SubtotalProducto;
            Impuesto = (SubTotal * 19) / 100;
            Total = SubTotal + Impuesto;
            DetalleFactura.Remove(detalle);

     
        }

        public void GuardarProducto()
        {
            ProductosModel productoSeleccionado = ListaProductos.FirstOrDefault(x=>x.Id== Detalle.IdProducto)!;
            Detalle.Imagen = productoSeleccionado.ImagenProducto!;
            Detalle.NombreProducto = productoSeleccionado.NombreProducto!;
            Detalle.PrecioUnitarioProducto = productoSeleccionado.PrecioUnitario;
            Detalle.SubtotalProducto = productoSeleccionado.PrecioUnitario * Detalle.CantidadDeProducto;
            SubTotal += Detalle.SubtotalProducto;
            Impuesto = (SubTotal * 19) / 100;
            Total = SubTotal + Impuesto;
            DetalleFactura.Add(Detalle);
            AdicionProductos = false;
            StateHasChanged();
        }
    }
}
