using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioPrestamosEquipo
    {
        Task<List<PrestamosEquipo>?> GetPrestamosEquipo();
        Task<PrestamosEquipo?> GetById(int id);
        Task<PrestamosEquipo?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO);
        Task<PrestamosEquipo?> Actualizar(int  id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO);
        Task<bool?> Eliminar(int id);
        Task<bool?> desactivarPrestamoEquipos(int id);
    }
}
