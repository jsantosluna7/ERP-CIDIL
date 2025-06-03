using ERP.Data.Modelos;
using Reservas.DTO.DTOHorario;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioHorario
    {
        Task<Horario?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO);
        Task<Horario?> AgregarHorario(CrearHorarioDTO crearHorarioDTO);
        Task<bool?> BorrarHorario(int id);
        Task<bool?> BorrarHorarioAutomatico(bool eliminar);
        Task<Horario?> ObtenerHorarioPorId(int id);
        Task<List<Horario>> ObtenerHorarios();
    }
}