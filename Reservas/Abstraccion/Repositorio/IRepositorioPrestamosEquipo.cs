using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioPrestamosEquipo
    {
        List<PrestamosEquipo> GetPrestamosEquipo();
        PrestamosEquipo GetById(int id);
        PrestamosEquipo Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO);
        PrestamosEquipo Actualizar(int  id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO);
        void Eliminar(int id);
    }
}
