using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioOrdenes
    {
        Task<Resultado<OrdenesDTO>> ActualizarOrdenes(int id, OrdenesDTO ordenesDTO);
        Task<Resultado<OrdenesDTO>> CrearOrdenes(OrdenesDTO ordene);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<OrdenesDTO>> ObtenerPorId(int id);
        Task<Resultado<List<OrdenesDTO>>> OrdenesAll();
    }
}