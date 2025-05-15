using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioReservaDeEspacio
    {
        Task<bool?> CancelarReserva(int id);
        Task<CrearReservaDeEspacioDTO?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<bool?> desactivarReservaDeEspacio(int id);
        Task<ActualizarReservaDeEspacioDTO?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<ReservaDeEspacioDTO?> ObtenerReservaPorId(int id);
        Task<List<ReservaDeEspacioDTO>?> ObtenerReservas();
    }
}