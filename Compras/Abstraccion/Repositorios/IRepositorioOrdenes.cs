using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioOrdenes
    {
        Task<Resultado<Ordene>> ActualizarOrdenes(int id, OrdenesDTO ordenesDTO);
        Task<Resultado<Ordene>> CrearOrdenes(OrdenesDTO ordene);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<Ordene>>> OrdenesAll();
    }
}