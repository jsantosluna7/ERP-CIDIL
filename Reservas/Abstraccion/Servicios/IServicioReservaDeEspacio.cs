using ERP.Data.Modelos;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioReservaDeEspacio
    {
        Task<Resultado<bool?>> CancelarReserva(int id);
        Task<Resultado<CrearReservaDeEspacioDTO?>> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<Resultado<bool?>> desactivarReservaDeEspacio(int id);
        Task<Resultado<ActualizarReservaDeEspacioDTO?>> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<Resultado<List<ReservaDeEspacio>>> ObtenerEspaciosUsuario(int id);
        Task<Resultado<ReservaDeEspacioDTO?>> ObtenerReservaPorId(int id);

        //Task<List<ReservaDeEspacioDTO>?> ObtenerReservas();
        Task<List<ReservaDeEspacioDTO>?> ObtenerReservas(int pagina, int tamanoPagina);
        Task<List<ReservaDeEspacioDTO>?> ObtenerReservasDeEspacioPorPiso(int piso);
        Task<List<ReservaDeEspacioDTO>?> ObtenerReservasTodo();
    }
}