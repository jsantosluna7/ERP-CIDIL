using ERP.Data.Modelos;
using Reservas.DTO.DTOEstado;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioEstado
    {
        Task<Resultado<Estado?>> GetById(int id);
        Task<Resultado<List<EstadoDTO>?>> GetEstado();
    }
}
