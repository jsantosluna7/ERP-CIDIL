using ERP.Data.Modelos;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioReservaDeEspacio
    {
        Task<Resultado<bool?>> CancelarReserva(int id);
        Task<Resultado<ReservaDeEspacio?>> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<Resultado<bool?>> desactivarReservaDeEspacio(int id);
        Task<Resultado<ReservaDeEspacio?>> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<Resultado<List<ReservaDeEspacio>>> ObtenerEspaciosUsuario(int id);
        Task<Resultado<ReservaDeEspacio?>> ObtenerReservaPorId(int id);

        //Task<List<ReservaDeEspacio>> ObtenerReservas();
        Task<List<ReservaDeEspacio>> ObtenerReservas(int pagina, int tamanoPagina);
        Task<List<ReservaDeEspacio>> ObtenerReservasDeEspacioPorPiso(int piso);
        Task<List<ReservaDeEspacio>> ObtenerReservasTodo();
    }
}