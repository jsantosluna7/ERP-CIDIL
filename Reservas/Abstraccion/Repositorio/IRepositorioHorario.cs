using ERP.Data.Modelos;
using Reservas.DTO.DTOHorario;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioHorario
    {
        Task<Horario?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO);
        //Task<(bool Exito, List<string> Errores)> AgregarHorariosAsync(List<CrearHorarioDTO> crearHorariosDTO);
        Task<(bool Exito, List<HorarioErrores> Errores)> AgregarHorariosAsync(List<CrearHorarioDTO> crearHorariosDTO);
        Task<bool?> BorrarHorario(int id);
        Task<bool?> BorrarHorarioAutomatico(bool eliminar);
        Task<Horario?> ObtenerHorarioPorId(int id);
        Task<List<Horario>> ObtenerHorarioPorPiso(int piso);
        Task<List<Horario>> ObtenerHorarios(int pagina, int tamanoPagina);
        Task<List<Horario>> ObtenerHorariosTotal();
    }
}