using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioOrdenes
    {
        Task<Resultado<List<OrdenesDTO>>> OrdenesAll();
    }
}