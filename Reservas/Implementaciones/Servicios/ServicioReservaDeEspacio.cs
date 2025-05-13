using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOHorario;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Modelos;

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
        public async Task<List<ReservaDeEspacioDTO>?> ObtenerReservas()
        {
            var reservas = await _repositorioReservaDeEspacio.ObtenerReservas();

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

        // Método para obtener las solicitudes de reserva
        public async Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservas()
        {
            var solicitudes = await _repositorioReservaDeEspacio.ObtenerSolicitudesReservas();

            if (solicitudes == null || solicitudes.Count == 0)
            {
                return null;
            }

            var solicitudesDTO = new List<SolicitudDeReservaDTO>();

            // Recorrer la lista de solicitudes y convertir cada una a SolicitudDeReservaDTO
            foreach (var solicitud in solicitudes)
            {
                var solicitudDTO = new SolicitudDeReservaDTO()
                {
                    Id = solicitud.Id,
                    IdUsuario = solicitud.IdUsuario,
                    IdLaboratorio = solicitud.IdLaboratorio,
                    HoraInicio = solicitud.HoraInicio,
                    HoraFinal = solicitud.HoraFinal,
                    Motivo = solicitud.Motivo,
                    FechaSolicitud = solicitud.FechaSolicitud,
                    IdEstado = solicitud.IdEstado
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                solicitudesDTO.Add(solicitudDTO);
            }

            return solicitudesDTO;
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
                IdEstado = reserva.IdEstado,
                Motivo = reserva.Motivo,
                FechaSolicitud = reserva.FechaSolicitud,
                IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                FechaAprobacion = reserva.FechaAprobacion,
                ComentarioAprobacion = reserva.ComentarioAprobacion
            };
            return reservaDTO;
        }

        // Método para obtener una solicitud de reserva por id
        public async Task<SolicitudDeReservaDTO?> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitud = await _repositorioReservaDeEspacio.ObtenerSolicitudReservaPorId(id);
            if (solicitud == null)
            {
                return null;
            }
            var solicitudDTO = new SolicitudDeReservaDTO()
            {
                Id = solicitud.Id,
                IdUsuario = solicitud.IdUsuario,
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud,
                IdEstado = solicitud.IdEstado
            };
            return solicitudDTO;
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

        // Método para solicitar una reserva
        public async Task<CrearSolicitudDeReservaDTO?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitud = await _repositorioReservaDeEspacio.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return null;
            }
            var solicitudDTO = new CrearSolicitudDeReservaDTO()
            {
                IdUsuario = solicitud.IdUsuario,
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud
            };
            return solicitudDTO;
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
                IdEstado = reserva.IdEstado,
                Motivo = reserva.Motivo,
                FechaSolicitud = reserva.FechaSolicitud,
                IdUsuarioAprobador = reserva.IdUsuarioAprobador,
                FechaAprobacion = reserva.FechaAprobacion,
                ComentarioAprobacion = reserva.ComentarioAprobacion
            };
            return reservaDTO;
        }

        // Método para editar una solicitud de reserva
        public async Task<ActualizarSolicitudDeReservaDTO?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitud = await _repositorioReservaDeEspacio.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return null;
            }
            var solicitudDTO = new ActualizarSolicitudDeReservaDTO()
            {
                IdLaboratorio = solicitud.IdLaboratorio,
                HoraInicio = solicitud.HoraInicio,
                HoraFinal = solicitud.HoraFinal,
                Motivo = solicitud.Motivo,
                FechaSolicitud = solicitud.FechaSolicitud
            };
            return solicitudDTO;
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

        // Método para cancelar una solicitud de reserva
        public async Task<bool?> CancelarSolicitudReserva(int id)
        {
            var resultado = await _repositorioReservaDeEspacio.CancelarSolicitudReserva(id);
            if (resultado == null)
            {
                return null;
            }
            return resultado;
        }
    }
}
