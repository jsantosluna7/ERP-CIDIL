using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOHorario;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Repositorios;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioReservaDeEspacio : IServicioReservaDeEspacio
    {
        private readonly IRepositorioReservaDeEspacio _repositorioReservaDeEspacio;
        public ServicioReservaDeEspacio(IRepositorioReservaDeEspacio repositorioReservaDeEspacio)
        {
            _repositorioReservaDeEspacio = repositorioReservaDeEspacio;
        }

        // Metodo para obtener todas las reservas
        public async Task<List<ReservaDeEspacioDTO>?> ObtenerReservas(int pagina, int tamanoPagina)
        {
            var reservas = await _repositorioReservaDeEspacio.ObtenerReservas(pagina, tamanoPagina);

            if (reservas == null || reservas.Count == 0)
            {
                return null;
            }
            var reservasDTO = new List<ReservaDeEspacioDTO>();

            // Recorrer la lista de reservas y convertir cada una a ReservaDeEspacioDTO
            foreach (var reserva in reservas)
            {
                var reservaDTO = new ReservaDeEspacioDTO()
                {
                    Id = reserva.Id,
                    IdUsuario = reserva.IdUsuario,
                    IdLaboratorio = reserva.IdLaboratorio,
                    HoraInicio = reserva.HoraInicio,
                    HoraFinal = reserva.HoraFinal,
                    FechaInicio = reserva.FechaInicio,
                    FechaFinal = reserva.FechaFinal,
                    IdEstado = reserva.IdEstado,
                    Motivo = reserva.Motivo,
                    FechaSolicitud = reserva.FechaSolicitud,
                    IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                    FechaAprobacion = reserva.FechaAprobacion,
                    ComentarioAprobacion = reserva.ComentarioAprobacion
                };

                // Agregar la reservaDTO a la lista de reservasDTO
                reservasDTO.Add(reservaDTO);
            }

            // Devolver la lista de reservasDTO
            return reservasDTO;
        }

        //Método para obtener todas las solicitudes de reserva por piso
        public async Task<List<ReservaDeEspacioDTO>?> ObtenerReservasDeEspacioPorPiso(int piso)
        {
            var reservas = await _repositorioReservaDeEspacio.ObtenerReservasDeEspacioPorPiso(piso);

            if (reservas == null || reservas.Count == 0)
            {
                return null;
            }

            var reservasDTO = new List<ReservaDeEspacioDTO>();

            // Recorrer la lista de solicitudes y convertir cada una a SolicitudDeReservaDTO
            foreach (var reserva in reservas)
            {
                var reservaDTO = new ReservaDeEspacioDTO()
                {
                    Id = reserva.Id,
                    IdUsuario = reserva.IdUsuario,
                    IdLaboratorio = reserva.IdLaboratorio,
                    HoraInicio = reserva.HoraInicio,
                    HoraFinal = reserva.HoraFinal,
                    FechaInicio = reserva.FechaInicio,
                    FechaFinal = reserva.FechaFinal,
                    Motivo = reserva.Motivo,
                    FechaSolicitud = reserva.FechaSolicitud,
                    IdEstado = reserva.IdEstado
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                reservasDTO.Add(reservaDTO);
            }

            return reservasDTO;
        }

        // Método para obtener una reserva por id
        public async Task<ReservaDeEspacioDTO?> ObtenerReservaPorId(int id)
        {
            var reserva = await _repositorioReservaDeEspacio.ObtenerReservaPorId(id);
            if (reserva == null)
            {
                return null;
            }
            var reservaDTO = new ReservaDeEspacioDTO()
            {
                Id = reserva.Id,
                IdUsuario = reserva.IdUsuario,
                IdLaboratorio = reserva.IdLaboratorio,
                HoraInicio = reserva.HoraInicio,
                HoraFinal = reserva.HoraFinal,
                FechaInicio = reserva.FechaInicio,
                FechaFinal = reserva.FechaFinal,
                IdEstado = reserva.IdEstado,
                Motivo = reserva.Motivo,
                FechaSolicitud = reserva.FechaSolicitud,
                IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                FechaAprobacion = reserva.FechaAprobacion,
                ComentarioAprobacion = reserva.ComentarioAprobacion
            };
            return reservaDTO;
        }

        // Método para crear una reserva
        public async Task<CrearReservaDeEspacioDTO?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO)
        {
            // Validar el DTO
            var reserva = await _repositorioReservaDeEspacio.CrearReserva(crearReservaDeEspacioDTO);
            if (reserva == null)
            {
                return null;
            }

            // Convertir la reserva a DTO
            var reservaDTO = new CrearReservaDeEspacioDTO()
            {
                IdUsuario = reserva.IdUsuario,
                IdLaboratorio = reserva.IdLaboratorio,
                HoraInicio = reserva.HoraInicio,
                HoraFinal = reserva.HoraFinal,
                FechaInicio = reserva.FechaInicio,
                FechaFinal = reserva.FechaFinal,
                IdEstado = reserva.IdEstado,
                Motivo = reserva.Motivo,
                FechaSolicitud = reserva.FechaSolicitud,
                IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                FechaAprobacion = reserva.FechaAprobacion,
                ComentarioAprobacion = reserva.ComentarioAprobacion
            };

            // Devolver la reservaDTO
            return reservaDTO;
        }

        // Método para editar una reserva
        public async Task<ActualizarReservaDeEspacioDTO?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO)
        {
            var reserva = await _repositorioReservaDeEspacio.EditarReserva(id, actualizarReservaDeEspacioDTO);
            if (reserva == null)
            {
                return null;
            }
            var reservaDTO = new ActualizarReservaDeEspacioDTO()
            {
                IdUsuario = reserva.IdUsuario,
                IdLaboratorio = reserva.IdLaboratorio,
                HoraInicio = reserva.HoraInicio,
                HoraFinal = reserva.HoraFinal,
                FechaInicio = reserva.FechaInicio,
                FechaFinal = reserva.FechaFinal,
                IdEstado = reserva.IdEstado,
                Motivo = reserva.Motivo,
                FechaSolicitud = reserva.FechaSolicitud,
                IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                FechaAprobacion = reserva.FechaAprobacion,
                ComentarioAprobacion = reserva.ComentarioAprobacion
            };
            return reservaDTO;
        }

        // Método para cancelar una reserva
        public async Task<bool?> CancelarReserva(int id)
        {
            var resultado = await _repositorioReservaDeEspacio.CancelarReserva(id);
            if (resultado == null)
            {
                return null;
            }
            return resultado;
        }

        //Método para desactivar un espacio
        public async Task<bool?> desactivarReservaDeEspacio(int id)
        {
            // Verificar si el espacio existe
            var espacio = await _repositorioReservaDeEspacio.ObtenerReservaPorId(id);
            if (espacio == null)
            {
                return null;
            }
            // Desactivar el espacio
            espacio.Activado = false;
            // Guardar los cambios en la base de datos
            await _repositorioReservaDeEspacio.desactivarReservaDeEspacio(id);
            return true;
        }
    }
}
