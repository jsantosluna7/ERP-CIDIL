using Reservas.DTO.DTOHorario;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioHorario
    {
        Task<ActualizarHorarioDTO?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO);
        Task<bool?> BorrarHorario(int id);
        Task<bool?> BorrarHorarioAutomatico(bool eliminar);
        Task<CrearHorarioDTO?> CrearHorario(CrearHorarioDTO crearHorarioDTO);
        Task<HorarioDTO?> ObtenerHorarioPorId(int id);
        Task<List<HorarioDTO>?> ObtenerHorarios(int pagina, int tamanoPagina);
    }
}