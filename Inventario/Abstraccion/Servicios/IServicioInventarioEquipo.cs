using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioInventarioEquipo
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        Task<List<InventarioEquipoDTO>?> GetInventarioEquipo();
        Task<InventarioEquipo?> GetById(int id);
        Task<InventarioEquipoDTO?> Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO);
        Task<InventarioEquipoDTO?> Actualizar(int id,ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO);
        Task<bool?> Eliminar(int id);
    }
}
