using Microsoft.AspNetCore.Components;
using Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevLabFront.Client.Pages
{
    public partial class Index
    {
        public FacturaModel Factura { get; set; } = new();
        public string? BaseAddressApi { get; set; }
        public List<ClientesModel> ListaClientes { get; set; } = new();
        [Inject] public HttpClient httpClient { get; set; } = new HttpClient();
        protected override async Task OnInitializedAsync()
        {
            BaseAddressApi = await Http.GetStringAsync("Inicio/ObtenerUrl");
            try
            {
                var request = JsonContent.Create(string.Empty);
                var response = await Http.PostAsync("Inicio/ObtenerClientesAsync", request);
                if (response.IsSuccessStatusCode)
                {
                    var lista = await response.Content.ReadAsStringAsync();
                    ListaClientes= JsonSerializer.Deserialize<List<ClientesModel>>(lista)!;
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

        }
    }
}
