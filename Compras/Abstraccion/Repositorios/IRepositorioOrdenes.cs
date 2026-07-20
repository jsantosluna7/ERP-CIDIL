using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioOrdenes
    {
        Task<Resultado<Ordene>> ActualizarOrdenes(int id, CrearOrdenesDTO ordenesDTO);
        Task<Resultado<Ordene>> CrearOrdenes(CrearOrdenesDTO ordene);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<Ordene>> ObtenerPorId(int id);
        Task<Resultado<List<Ordene>>> OrdenesAll();
    }
}