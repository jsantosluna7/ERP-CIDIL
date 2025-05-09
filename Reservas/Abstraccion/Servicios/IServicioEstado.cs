using Reservas.DTO.DTOEstado;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioEstado
    {
        List<EstadoDTO> GetEstado();
        Estado GetById(int id);
    }
}
