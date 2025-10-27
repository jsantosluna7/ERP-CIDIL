using ERP.Data.Modelos;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioPrestamosEquipo
    {
        //Task<List<PrestamosEquipo>?> GetPrestamosEquipo();
        Task<PrestamosEquipo?> GetById(int id);
        Task<PrestamosEquipo?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO);
        Task<PrestamosEquipo?> Actualizar(int  id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO);
        Task<bool?> Eliminar(int id);
        Task<bool?> desactivarPrestamoEquipos(int id);
        Task<List<PrestamosEquipo>?> GetPrestamosEquipo(int pagina, int tamanoPagina);
        Task<Resultado<List<PrestamosEquipo>>> ObtenerEquiposUsuario(int id);
    }
}
