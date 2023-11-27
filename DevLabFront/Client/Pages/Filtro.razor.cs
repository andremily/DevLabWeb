using Microsoft.AspNetCore.Components;
using Models;
using System.Net.Http.Json;
using System.Text.Json;


namespace DevLabFront.Client.Pages
{
    public partial class Filtro
    {
        [Inject] public HttpClient httpClient { get; set; } = new HttpClient();
        public string? BaseAddressApi { get; set; }
        public string? Filtros { get; set; } = "Id Cliente";
        public bool DesHabilitadoCliente { get; set; } = false;
        public bool DesHabilitadoFactura { get; set; } = true;
        public FiltroModel RequestFiltro { set; get; } = new();
        public int? IdCliente { get; set; }
        public int? NumeroFactura { get; set; }
        public List<string> Mensaje { get; set; } = new();
        public bool AbrirMensaje { get; set; } = false;
        public string? Titulo { get; set; }
        public List<FacturaModel> Facturas { get; set; }
        protected override async Task OnInitializedAsync()
        {
            BaseAddressApi = await httpClient.GetStringAsync("Inicio/ObtenerUrl");
        }
        private void OnFiltrosChanged(string value)
        {
            Filtros = value;
            if (Filtros == "Id Cliente")
            {
                DesHabilitadoCliente = false; 
                DesHabilitadoFactura = true;
                RequestFiltro.NumeroFactura = null;
            }
            else
            {
                DesHabilitadoCliente = true;
                DesHabilitadoFactura = false;
                RequestFiltro.IdCliente = null;
            }
            StateHasChanged();  
        }

        private async Task BuscarAsync()
        {
            try
            {
                if (Filtros == "Id Cliente" && RequestFiltro.IdCliente is null)
                {
                    Titulo = "Error";
                    Mensaje.Add("El filtro de CLiente no puede estar vacio");
                }
                if (Filtros == "Numero Factura" && RequestFiltro.IdCliente is null)
                {
                    Titulo = "Error";
                    Mensaje.Add("El filtro de número factura no puede estar vacio");
                }
                var request = JsonContent.Create(RequestFiltro);
                var response = await httpClient.PostAsync("Inicio/ConsultarFacturaAsync", request);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    Facturas = JsonSerializer.Deserialize<List<FacturaModel>>(stringResponse)!;
                    StateHasChanged();
                }
                else
                {

                    ManejoMensajes(new Response { Resultado = false, Mensaje = $"{response.StatusCode} - {response.ReasonPhrase}" });
                }

            }
            catch (Exception ex)
            {
                ManejoMensajes(new Response { Resultado = false, Mensaje = ex.Message });
            }

        }
        private void ManejoMensajes(Response respuesta)
        {
            if (respuesta.Resultado)
            {
                Titulo = "Operación Exitosa";
                Mensaje.Add(respuesta.Mensaje);
                AbrirMensaje = true;
             
            }
            else
            {
                Titulo = "Error";
                Mensaje.Add(respuesta.Mensaje);
                AbrirMensaje = true;
            }
        }
    }
}
