using ERP.Data.Modelos;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioReservaDeEspacio
    {
        Task<bool?> CancelarReserva(int id);
        Task<ReservaDeEspacio?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<bool?> desactivarReservaDeEspacio(int id);
        Task<ReservaDeEspacio?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<ReservaDeEspacio?> ObtenerReservaPorId(int id);
        //Task<List<ReservaDeEspacio>> ObtenerReservas();
        Task<List<ReservaDeEspacio>> ObtenerReservas(int pagina, int tamanoPagina);
    }
}