using ERP.Data.Modelos;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioPrestamosEquipo
    {
         //Task<List<PrestamosEquipoDTO>?> GetPrestamosEquipo();
         Task<PrestamosEquipo?> GetById(int id);
         Task<PrestamosEquipoDTO?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO);
         Task<PrestamosEquipoDTO?> Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO);
         Task<bool?> Eliminar(int  id);
        Task<bool?> DesactivarPrestamoEquipos(int id);
        Task<List<PrestamosEquipoDTO>?> GetPrestamosEquipo(int pagina, int tamanoPagina);
        Task<Resultado<List<PrestamosEquipo>>> ObtenerEquiposUsuario(int id);
    }
}
