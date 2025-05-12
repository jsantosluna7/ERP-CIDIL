using Inventario.DTO.EstadoFisicoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioEstadoFisico
    {
        Task<List<EstadoFisicoDTO>?> GetEstadoFisico();
        Task<EstadoFisico?> GetById(int id);
    }
}
