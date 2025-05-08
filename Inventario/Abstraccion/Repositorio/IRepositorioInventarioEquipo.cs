using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;

namespace Inventario.Abstraccion.Repositorio
{
    public interface IRepositorioInventarioEquipo
    {

        //Creamos los metodos para Crear, actualizar, Eliminar y Optener
        List<InventarioEquipo> GetInventarioEquipos();
        InventarioEquipo GetById(int id);
        InventarioEquipo Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO);
        InventarioEquipo Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO);
        void Eliminar(int  id);
    }
}
