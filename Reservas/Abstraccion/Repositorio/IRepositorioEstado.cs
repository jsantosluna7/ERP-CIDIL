using Reservas.DTO.DTOEstado;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioEstado
    {
        List<Estado> GetEstado();
        Estado GetById(int id);
    }
}
