using ERP.Data.Modelos;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioSolicitudPrestamosDeEquipos : IServicioSolicitudPrestamosDeEquipos
    {
        private readonly IRepositorioSolicitudPrestamosDeEquipos _repositorio;
        private readonly ServicioEmailReservas _servicioEmail;
        private readonly DbErpContext _context;

        private const int ESTADO_PENDIENTE = 2;
        private const int ESTADO_APROBADO = 1;
        private static readonly List<int> ESTADOS_QUE_BLOQUEAN_STOCK = new() { ESTADO_APROBADO };


        public ServicioSolicitudPrestamosDeEquipos(
            IRepositorioSolicitudPrestamosDeEquipos repositorio,
            ServicioEmailReservas servicioEmail,
            DbErpContext context)
        {
            _repositorio = repositorio;
            _servicioEmail = servicioEmail;
            _context = context;
        }

        public async Task<Resultado<List<SolicitudPrestamosDeEquiposDTO>>> ObtenerTodas(int pagina, int tamanoPagina)
        {
            var solicitudes = await _repositorio.ObtenerTodas(pagina, tamanoPagina);
            return Resultado<List<SolicitudPrestamosDeEquiposDTO>>.Exito(
                solicitudes.ToList());
        }

        public async Task<Resultado<SolicitudPrestamosDeEquiposDTO>> ObtenerPorId(int id)
        {
            var solicitud = await _repositorio.ObtenerPorIdTodo(id);
            if (solicitud == null)
                return Resultado<SolicitudPrestamosDeEquiposDTO>.Falla($"No se encontró la solicitud con ID {id}.");

            return Resultado<SolicitudPrestamosDeEquiposDTO>.Exito(solicitud);
        }

        public async Task<Resultado<List<SolicitudPrestamosDeEquiposDTO>>> ObtenerPorUsuario(int idUsuario)
        {
            var solicitudes = await _repositorio.ObtenerPorUsuario(idUsuario);
            if (!solicitudes.Any())
                return Resultado<List<SolicitudPrestamosDeEquiposDTO>>.Falla("No se encontraron solicitudes para este usuario.");

            return Resultado<List<SolicitudPrestamosDeEquiposDTO>>.Exito(
                solicitudes.ToList());
        }

        // El método ya no retorna Resultado<List<...>> sino el nuevo DTO detallado
        public async Task<ResultadoCrearMultiplesDTO> CrearMultiples(CrearSolicitudPrestamosDeEquiposDTO dto)
        {
            var respuesta = new ResultadoCrearMultiplesDTO();

            if (dto.Equipos == null || !dto.Equipos.Any())
            {
                respuesta.TodosExitosos = false;
                respuesta.Resultados.Add(new ResultadoItemSolicitudDTO
                {
                    Exitoso = false,
                    Error = "Debe incluir al menos un equipo."
                });
                return respuesta;
            }

            // Validaciones de fechas (rápido, sin tocar BD)
            foreach (var item in dto.Equipos)
            {
                if (item.FechaInicio >= item.FechaFinal)
                {
                    respuesta.Resultados.Add(new ResultadoItemSolicitudDTO
                    {
                        IdInventario = item.IdInventario,
                        Exitoso = false,
                        Error = "La fecha de inicio debe ser anterior a la fecha final."
                    });
                }
                else if (item.FechaInicio < DateTime.Now)
                {
                    respuesta.Resultados.Add(new ResultadoItemSolicitudDTO
                    {
                        IdInventario = item.IdInventario,
                        Exitoso = false,
                        Error = "La fecha de inicio no puede ser en el pasado."
                    });
                }
                else
                {
                    // Sin error de fecha, marcamos como pendiente de validar stock
                    respuesta.Resultados.Add(new ResultadoItemSolicitudDTO
                    {
                        IdInventario = item.IdInventario,
                        Exitoso = true // puede cambiar en la validación de stock
                    });
                }
            }

            // Si ya hay errores de fecha, retornamos sin tocar la BD
            if (respuesta.Resultados.Any(r => !r.Exitoso))
            {
                respuesta.TodosExitosos = false;
                return respuesta;
            }

            // Validación de stock para cada ítem (sin transacción aún)
            for (int i = 0; i < dto.Equipos.Count; i++)
            {
                var item = dto.Equipos[i];
                var resultadoItem = respuesta.Resultados[i];

                var inventario = await _repositorio.ObtenerInventarioPorId(item.IdInventario);
                if (inventario == null)
                {
                    resultadoItem.Exitoso = false;
                    resultadoItem.Error = $"No existe el equipo con ID {item.IdInventario} en inventario.";
                    continue;
                }

                resultadoItem.NombreEquipo = inventario.Nombre;

                int cantidadYaReservada = await _repositorio.ObtenerCantidadReservadaEnRango(
                    item.IdInventario, item.FechaInicio, item.FechaFinal, ESTADOS_QUE_BLOQUEAN_STOCK);

                int stockDisponible = (inventario.Cantidad ?? 0) - cantidadYaReservada;

                if (item.Cantidad > stockDisponible)
                {
                    resultadoItem.Exitoso = false;
                    resultadoItem.Error = $"Stock insuficiente. Disponible: {stockDisponible}, Solicitado: {item.Cantidad}.";
                }
            }

            // Si algún ítem falló la validación, retornamos todos los resultados sin guardar nada
            if (respuesta.Resultados.Any(r => !r.Exitoso))
            {
                respuesta.TodosExitosos = false;
                return respuesta;
            }

            // Todo válido — ahora sí guardamos en transacción
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var solicitudesCreadas = new List<SolicitudPrestamosDeEquipo>();

                for (int i = 0; i < dto.Equipos.Count; i++)
                {
                    var item = dto.Equipos[i];

                    var nuevaSolicitud = new SolicitudPrestamosDeEquipo
                    {
                        IdUsuario = dto.IdUsuario,
                        IdInventario = item.IdInventario,
                        FechaInicio = item.FechaInicio,
                        FechaFinal = item.FechaFinal,
                        Motivo = item.Motivo,
                        FechaSolicitud = dto.FechaSolicitud,
                        Cantidad = item.Cantidad,
                        IdEstado = ESTADO_PENDIENTE
                    };

                    await _repositorio.Crear(nuevaSolicitud);
                    solicitudesCreadas.Add(nuevaSolicitud);
                    respuesta.Resultados[i].Solicitud = MapearDTO(nuevaSolicitud);
                }

                await _repositorio.GuardarCambios();

                // Emails a admins
                var admins = await _repositorio.ObtenerAdmins();
                foreach (var solicitud in solicitudesCreadas)
                {
                    var inventario = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    string fechaInicio = solicitud.FechaInicio.ToString("dd/MM/yyyy h:mm tt");
                    string fechaFinal = solicitud.FechaFinal.ToString("dd/MM/yyyy h:mm tt");

                    foreach (var admin in admins)
                    {
                        await _servicioEmail.EnviarCorreoReservaEquipos(
                            admin.CorreoInstitucional,
                            inventario!.Nombre,
                            solicitud.Cantidad.ToString(),
                            fechaInicio,
                            fechaFinal);
                    }
                }

                await transaccion.CommitAsync();
                respuesta.TodosExitosos = true;
                return respuesta;
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                respuesta.TodosExitosos = false;
                respuesta.Resultados.ForEach(r =>
                {
                    r.Exitoso = false;
                    r.Error = $"Error interno al guardar: {ex.Message}";
                });
                return respuesta;
            }
        }

        public async Task<Resultado<SolicitudPrestamosDeEquiposDTO>> Actualizar(int id, ActualizarSolicitudPrestamosDeEquiposDTO dto)
        {
            var solicitud = await _repositorio.ObtenerPorId(id);
            if (solicitud == null)
                return Resultado<SolicitudPrestamosDeEquiposDTO>.Falla($"No se encontró la solicitud con ID {id}.");

            if (solicitud.IdEstado != ESTADO_PENDIENTE)
                return Resultado<SolicitudPrestamosDeEquiposDTO>.Falla(
                    "Solo se pueden modificar solicitudes en estado Pendiente.");

            var inventario = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
            if (inventario == null)
                return Resultado<SolicitudPrestamosDeEquiposDTO>.Falla("No se encontró el inventario asociado.");

            int cantidadYaReservada = await _repositorio.ObtenerCantidadReservada(
                solicitud.IdInventario, dto.FechaInicio, dto.FechaFinal, excludeId: id);

            int stockDisponible = (inventario.Cantidad ?? 0) - cantidadYaReservada;

            if (dto.Cantidad > stockDisponible)
                return Resultado<SolicitudPrestamosDeEquiposDTO>.Falla(
                    $"Stock insuficiente para '{inventario.Nombre}'. " +
                    $"Disponible en ese rango de fechas: {stockDisponible}, Solicitado: {dto.Cantidad}.");


            solicitud.FechaInicio = dto.FechaInicio;
            solicitud.FechaFinal = dto.FechaFinal;
            solicitud.Motivo = dto.Motivo;
            solicitud.Cantidad = dto.Cantidad;

            await _repositorio.Actualizar(solicitud);
            await _repositorio.GuardarCambios();

            return Resultado<SolicitudPrestamosDeEquiposDTO>.Exito(MapearDTO(solicitud));
        }


        public async Task<Resultado<bool>> Eliminar(int id)
        {
            var solicitud = await _repositorio.ObtenerPorId(id);
            if (solicitud == null)
                return Resultado<bool>.Falla($"No se encontró la solicitud con ID {id}.");

            if (solicitud.IdEstado != ESTADO_PENDIENTE)
                return Resultado<bool>.Falla("Solo se pueden cancelar solicitudes en estado Pendiente.");

            await _repositorio.Eliminar(solicitud.Id);
            await _repositorio.GuardarCambios();

            return Resultado<bool>.Exito(true);
        }


        private static SolicitudPrestamosDeEquiposDTO MapearDTO(SolicitudPrestamosDeEquipo s) => new()
        {
            Id = s.Id,
            IdUsuario = s.IdUsuario,
            IdInventario = s.IdInventario,
            FechaInicio = s.FechaInicio,
            FechaFinal = s.FechaFinal,
            Motivo = s.Motivo,
            FechaSolicitud = s.FechaSolicitud,
            IdEstado = s.IdEstado,
            Cantidad = s.Cantidad,
        };
    }
}
