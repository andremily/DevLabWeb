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
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
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

        public async Task AdicionarProductosAsync()
        {
            await ObtenerProductos();
            Detalle = new();
            AdicionProductos = true;
            StateHasChanged();

        }
        private void SeleccionProducto()
        {

        }
        public void Eliminar(DetalleFacturaModel detalle)
        {
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
            Iva = (SubTotal * 19) / 100;
            DetalleFactura.Add(Detalle);
        }
    }
}
