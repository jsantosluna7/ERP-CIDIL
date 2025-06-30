using Reservas.DTO.DTOHorario;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioHorario
    {
        Task<ActualizarHorarioDTO?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO);
        Task<bool?> BorrarHorario(int id);
        Task<bool?> BorrarHorarioAutomatico(bool eliminar);
        //Task<(bool Exito, List<string> Errores)> CrearHorariosDesdeLista(List<CrearHorarioDTO> listaHorarios);
        Task<(bool Exito, List<HorarioErrores> Errores)> CrearHorariosDesdeLista(List<CrearHorarioDTO> listaHorarios);
        Task<HorarioDTO?> ObtenerHorarioPorId(int id);
        Task<List<HorarioDTO>?> ObtenerHorarios(int pagina, int tamanoPagina);
        Task<List<HorarioDTO>?> ObtenerHorariosTotal();
    }
}