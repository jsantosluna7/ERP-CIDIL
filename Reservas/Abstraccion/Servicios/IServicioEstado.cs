using ERP.Data.Modelos;
using Reservas.DTO.DTOEstado;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioEstado
    {
        Task<List<EstadoDTO>?> GetEstado();
        Task<Estado?> GetById(int id);
    }
}
