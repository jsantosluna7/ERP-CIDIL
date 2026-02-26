using ERP.Data.Modelos;
using Inventario.DTO.InventarioEquipoDTO;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioInventarioEquipo
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        //Task<List<InventarioEquipoDTO>?> GetInventarioEquipo();
        Task<InventarioEquipo?> GetById(int id);
        Task<bool?> Eliminar(int id);
        Task<bool?> DesactivarEquipo(int id);
        Task<List<InventarioEquipoDTO>?> GetInventarioEquipo(int pagina, int tamanoPagina);
        Task<Resultado<List<InventarioEquipo>>> BuscarPorNombre(string nombre);
        Task<InventarioEquipoDTO?> Actualizar(int id, ActualizarInventarioEquipoDTO dto);
        Task<InventarioEquipoDTO?> Crear(CrearInventarioEquipoDTO dto);
    }
}
