using ERP.Data.Modelos;
using Reservas.DTO.DTOTotalReservaEspacios;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioTotalReservaDeEspacio
    {
        Task<Resultado<ConteoReservaEspaciosDTO>> ObtenerConteoReservasDeEspacios();
        Task<Resultado<List<ReservaDeEspacioUsuarioDTO>>> ObtenerTodasLasReservasDeEspacioDelUsuario(int idUsuario, int? idEstado = null);
        Task<Resultado<List<ReservaEspaciosAdminDTO>>> ObtenerTodasLasReservasDeEspacios(int? idEstado = null, string? busqueda = null, int pagina = 1, int tamanoPagina = 20);
    }
}