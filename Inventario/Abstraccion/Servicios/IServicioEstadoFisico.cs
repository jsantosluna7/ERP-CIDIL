using Inventario.DTO.EstadoFisicoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    //Creamos los metodos para Optener
    public interface IServicioEstadoFisico
    {
        Task<List<EstadoFisicoDTO>?> GetEstadoFisico();
        Task<EstadoFisico?> GetById(int id);
    }
}
