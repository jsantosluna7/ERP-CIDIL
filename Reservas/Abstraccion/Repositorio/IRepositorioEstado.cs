using Reservas.DTO.DTOEstado;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioEstado
    {
        Task<List<Estado>?> GetEstado();
        Task<Estado?> GetById(int id);
    }
}
