using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Repositorio
{
    public interface IRepositorioInventarioEquipo
    {

        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
         //Task<List<InventarioEquipo>?> GetInventarioEquipos();
         Task<InventarioEquipo?> GetById(int id);
         Task<InventarioEquipo?> Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO);
         Task<InventarioEquipo?> Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO);
         Task<bool?> Eliminar(int  id);
        Task<bool?> DesactivarEquipo(int id);
        Task<List<InventarioEquipo>?> GetInventarioEquipos(int pagina, int tamanoPagina);
    }
}
