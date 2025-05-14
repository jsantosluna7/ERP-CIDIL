using Inventario.Modelos;

namespace Inventario.Abstraccion.Repositorio
{
    //Creamos los metodos para  Optener
    public interface IRepositorioEstadoFisico
    {
        Task<List<EstadoFisico>?> GetEstadoFisico();
        Task<EstadoFisico?> GetById(int id);
    }
}
