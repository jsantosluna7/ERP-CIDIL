using ERP.Data.Modelos;
using Reservas.DTO.DTOEstado;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioEstado
    {
        Task<List<Estado>?> GetEstado();
        Task<Resultado<Estado?>> GetById(int id);
    }
}
