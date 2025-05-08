using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Servicios
{
    public interface IServicioInventarioEquipo
    {
        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        List<InventarioEquipoDTO> GetInventarioEquipo();
        InventarioEquipo GetById(int id);
        InventarioEquipoDTO Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO);
        InventarioEquipoDTO Actualizar(int id,ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO);
        void Eliminar(int id);
    }
}
