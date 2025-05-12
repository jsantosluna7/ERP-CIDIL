using Inventario.Modelos;

namespace Inventario.Abstraccion.Repositorio
{
    public interface IRepositorioEstadoFisico
    {
        Task<List<EstadoFisico>?> GetEstadoFisico();
        Task<EstadoFisico?> GetById(int id);
    }
}
