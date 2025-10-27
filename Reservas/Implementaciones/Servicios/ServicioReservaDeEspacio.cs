using ERP.Data.Modelos;
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
                    ComentarioAprobacion = reserva.ComentarioAprobacion,
                    PersonasCantidad = reserva.PersonasCantidad
                };

                // Agregar la reservaDTO a la lista de reservasDTO
                reservasDTO.Add(reservaDTO);
            }

            // Devolver la lista de reservasDTO
            return reservasDTO;
        }

        // Metodo para obtener todas las reservas
        public async Task<List<ReservaDeEspacioDTO>?> ObtenerReservasTodo()
        {
            var reservas = await _repositorioReservaDeEspacio.ObtenerReservasTodo();

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
                    ComentarioAprobacion = reserva.ComentarioAprobacion,
                    PersonasCantidad = reserva.PersonasCantidad
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
                    IdEstado = reserva.IdEstado,
                    PersonasCantidad = reserva.PersonasCantidad
                };
                // Agregar la solicitudDTO a la lista de solicitudesDTO
                reservasDTO.Add(reservaDTO);
            }

            return reservasDTO;
        }

        // Método para obtener una reserva por id
        public async Task<Resultado<ReservaDeEspacioDTO?>> ObtenerReservaPorId(int id)
        {
            var reservaPorId = await _repositorioReservaDeEspacio.ObtenerReservaPorId(id);
            var reserva = reservaPorId.Valor;
            if (!reservaPorId.esExitoso)
            {
                return Resultado<ReservaDeEspacioDTO?>.Falla(reservaPorId.MensajeError ?? "No se pudo obtener la reserva por id.");
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
                ComentarioAprobacion = reserva.ComentarioAprobacion,
                PersonasCantidad = reserva.PersonasCantidad
            };
            return Resultado<ReservaDeEspacioDTO?>.Exito(reservaDTO);
        }

        public async Task<Resultado<List<ReservaDeEspacio>>> ObtenerEspaciosUsuario(int id)
        {
            var resultado = await _repositorioReservaDeEspacio.ObtenerEspaciosUsuario(id);
            if (!resultado.esExitoso)
            {
                return Resultado<List<ReservaDeEspacio>>.Falla(resultado.MensajeError ?? "Error desconocido al obtener los espacios del usuario.");
            }

            return Resultado<List<ReservaDeEspacio>>.Exito(resultado.Valor!);
        }

        // Método para crear una reserva
        public async Task<Resultado<CrearReservaDeEspacioDTO?>> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO)
        {
            // Validar el DTO
            var reservaPorId = await _repositorioReservaDeEspacio.CrearReserva(crearReservaDeEspacioDTO);
            var reserva = reservaPorId.Valor;

            if (!reservaPorId.esExitoso)
            {
                return Resultado<CrearReservaDeEspacioDTO?>.Falla(reservaPorId.MensajeError ?? "No se pudo crear la reserva.");
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
                ComentarioAprobacion = reserva.ComentarioAprobacion,
                PersonasCantidad = reserva.PersonasCantidad
            };

            // Devolver la reservaDTO
            return Resultado<CrearReservaDeEspacioDTO?>.Exito(reservaDTO);
        }

        // Método para editar una reserva
        public async Task<Resultado<ActualizarReservaDeEspacioDTO?>> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO)
        {
            var reservaPorId = await _repositorioReservaDeEspacio.EditarReserva(id, actualizarReservaDeEspacioDTO);
            var reserva = reservaPorId.Valor;

            if (!reservaPorId.esExitoso)
            {
                return Resultado<ActualizarReservaDeEspacioDTO?>.Falla(reservaPorId.MensajeError ?? "No se pudo actualizar la reserva de espacio.");
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
                ComentarioAprobacion = reserva.ComentarioAprobacion,
                PersonasCantidad = reserva.PersonasCantidad
            };
            return Resultado<ActualizarReservaDeEspacioDTO?>.Exito(reservaDTO);
        }

        // Método para cancelar una reserva
        public async Task<Resultado<bool?>> CancelarReserva(int id)
        {
            var resultadoPorId = await _repositorioReservaDeEspacio.CancelarReserva(id);
            var resultado = resultadoPorId.Valor;

            if (!resultadoPorId.esExitoso)
            {
                return Resultado<bool?>.Falla(resultadoPorId.MensajeError ?? "No se pudo cancelar la reserva");
            }
            return Resultado<bool?>.Exito(resultado);
        }

        //Método para desactivar un espacio
        public async Task<Resultado<bool?>> desactivarReservaDeEspacio(int id)
        {
            // Verificar si el espacio existe
            var espacioPorId = await _repositorioReservaDeEspacio.ObtenerReservaPorId(id);

            var espacio = espacioPorId.Valor;

            if (!espacioPorId.esExitoso)
            {
                return Resultado<bool?>.Falla(espacioPorId.MensajeError ?? "No se pudo obtener la reserva por id.");
            }
            // Desactivar el espacio
            espacio.Activado = false;
            // Guardar los cambios en la base de datos
            await _repositorioReservaDeEspacio.desactivarReservaDeEspacio(id);
            return Resultado<bool?>.Exito(true);
        }
    }
}
