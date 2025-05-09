using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioPrestamosEquipo
    {
        List<PrestamosEquipoDTO> GetPrestamosEquipo();
        PrestamosEquipo GetById(int id);
        PrestamosEquipoDTO Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO);
        PrestamosEquipoDTO Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO);
        void Eliminar(int  id);
    }
}
