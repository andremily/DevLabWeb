using Models;

namespace DevLabWebApi.Services
{
    public interface IFactura
    {
         List<FacturaModel> GetFacturaList(int? numeroFactura, int? idCliente);
        Response GuardarFactura(FacturaCompleta factura);


    }
}
