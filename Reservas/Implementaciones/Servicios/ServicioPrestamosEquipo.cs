using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioPrestamosEquipo : IServicioPrestamosEquipo
    {
        private readonly IRepositorioPrestamosEquipo _repositorio;
        private readonly ServicioEmailReservas _servicioEmail;
        private readonly DbErpContext _context;

        private const int ESTADO_APROBADO = 1;
        private const int ESTADO_PENDIENTE = 2;
        private const int ESTADO_RECHAZADO = 3;
        private const int ESTADO_DEVUELTO = 4;
        private const int ESTADO_EXTENSION_SOLICITADA = 5;

        public ServicioPrestamosEquipo(
            IRepositorioPrestamosEquipo repositorio,
            ServicioEmailReservas servicioEmail,
            DbErpContext context)
        {
            _repositorio = repositorio;
            _servicioEmail = servicioEmail;
            _context = context;
        }

        // ── GET consolidado ───────────────────────────────────────────────────────

        /// <summary>
        /// estado: null = todos | "pendiente" | "activo" | "atrasado"
        /// idUsuario: null = todos los usuarios (admin) | valor = filtrar por usuario
        /// </summary>
        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPrestamos(
            string? estado = null,
            int? idUsuario = null,
            int pagina = 1,
            int tamanoPagina = 20)
        {
            var estadosValidos = new[] { "pendiente", "activo", "atrasado" };
            if (!string.IsNullOrWhiteSpace(estado) &&
                !estadosValidos.Contains(estado.ToLower()))
            {
                return Resultado<List<PrestamosEquipoDTO>>.Falla(
                    $"Estado '{estado}' no válido. Use: {string.Join(", ", estadosValidos)}.");
            }

            List<PrestamosEquipoDTO> lista;

            // Si se pide un usuario específico, ignorar el filtro de estado
            if (idUsuario.HasValue)
            {
                lista = await _repositorio.ObtenerPorUsuario(idUsuario.Value);
                if (!lista.Any())
                    return Resultado<List<PrestamosEquipoDTO>>.Falla("No se encontraron préstamos para este usuario.");

                return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
            }

            lista = estado?.ToLower() switch
            {
                "pendiente" => await _repositorio.ObtenerPendientes(),
                "activo" => await _repositorio.ObtenerActivos(),
                "atrasado" => await _repositorio.ObtenerAtrasados(),
                _ => await _repositorio.ObtenerTodos(pagina, tamanoPagina)
            };

            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
        }

        public async Task<Resultado<PrestamosEquipoDTO>> ObtenerPorId(int id)
        {
            var prestamo = await _repositorio.ObtenerPorId(id);
            if (prestamo == null)
                return Resultado<PrestamosEquipoDTO>.Falla($"No se encontró el préstamo con ID {id}.");

            return Resultado<PrestamosEquipoDTO>.Exito(CalcularAtraso(MapearDTO(prestamo)));
        }

        public async Task<Resultado<ResumenPrestamosDTO>> ObtenerResumen()
        {
            var pendientes = await _repositorio.ObtenerPendientes();
            var activos = await _repositorio.ObtenerActivos();
            var atrasados = await _repositorio.ObtenerAtrasados();
            var extensiones = await _repositorio.ObtenerExtensionsPendientes();

            return Resultado<ResumenPrestamosDTO>.Exito(new ResumenPrestamosDTO
            {
                TotalPendientes = pendientes.Count,
                TotalActivos = activos.Count,
                TotalAtrasados = atrasados.Count,
                TotalExtensionsSolicitadas = extensiones.Count
            });
        }

        // ── POST consolidado ──────────────────────────────────────────────────────

        /// <summary>
        /// Despacha a ProcesarSolicitud o AprobarRechazarExtension
        /// según dto.TipoAccion ("solicitud" | "extension").
        /// </summary>
        public async Task<Resultado<object>> Procesar(ProcesarPrestamosEquipoDTO dto)
        {
            switch (dto.TipoAccion?.ToLower())
            {
                case "solicitud":
                    if (!dto.IdSolicitud.HasValue)
                        return Resultado<object>.Falla("IdSolicitud es requerido para TipoAccion 'solicitud'.");

                    var dtoSolicitud = new AprobarRechazarSolicitudDTO
                    {
                        IdSolicitud = dto.IdSolicitud.Value,
                        IdUsuarioAprobador = dto.IdUsuarioAprobador,
                        Aprobado = dto.Aprobado,
                        ComentarioAprobacion = dto.ComentarioAprobacion
                    };
                    var resSolicitud = await ProcesarSolicitud(dtoSolicitud);
                    if (!resSolicitud.esExitoso)
                        return Resultado<object>.Falla(resSolicitud.MensajeError!);

                    return Resultado<object>.Exito(
                        resSolicitud.Valor != null
                            ? (object)resSolicitud.Valor
                            : new { mensaje = "Solicitud rechazada correctamente." });

                case "extension":
                    if (!dto.IdExtension.HasValue)
                        return Resultado<object>.Falla("IdExtension es requerido para TipoAccion 'extension'.");

                    var dtoExtension = new AprobarRechazarExtensionDTO
                    {
                        IdUsuarioAprobador = dto.IdUsuarioAprobador,
                        Aprobado = dto.Aprobado,
                        ComentarioAprobacion = dto.ComentarioAprobacion
                    };
                    var resExtension = await AprobarRechazarExtension(dto.IdExtension.Value, dtoExtension);
                    if (!resExtension.esExitoso)
                        return Resultado<object>.Falla(resExtension.MensajeError!);

                    return Resultado<object>.Exito((object)resExtension.Valor!);

                default:
                    return Resultado<object>.Falla("TipoAccion inválido. Use: 'solicitud' o 'extension'.");
            }
        }

        // ── Sin cambios desde aquí ────────────────────────────────────────────────

        public async Task<Resultado<PrestamosEquipoDTO>> MarcarDevuelto(int id, MarcarDevueltoDTO dto)
        {
            var prestamo = await _repositorio.ObtenerPorId(id);
            if (prestamo == null)
                return Resultado<PrestamosEquipoDTO>.Falla($"No se encontró el préstamo con ID {id}.");

            if (prestamo.IdEstado == ESTADO_DEVUELTO)
                return Resultado<PrestamosEquipoDTO>.Falla("Este préstamo ya fue marcado como devuelto.");

            if (prestamo.IdEstado != ESTADO_APROBADO && prestamo.IdEstado != ESTADO_EXTENSION_SOLICITADA)
                return Resultado<PrestamosEquipoDTO>.Falla("Solo se pueden marcar como devueltos préstamos activos.");

            prestamo.FechaEntrega = dto.FechaEntrega;
            prestamo.IdEstado = ESTADO_DEVUELTO;

            await _repositorio.Actualizar(prestamo);
            await _repositorio.GuardarCambios();

            return Resultado<PrestamosEquipoDTO>.Exito(CalcularAtraso(MapearDTO(prestamo)));
        }

        public async Task<Resultado<bool>> Eliminar(int id)
        {
            var prestamo = await _repositorio.ObtenerPorId(id);
            if (prestamo == null)
                return Resultado<bool>.Falla($"No se encontró el préstamo con ID {id}.");

            await _repositorio.Eliminar(prestamo);
            await _repositorio.GuardarCambios();
            return Resultado<bool>.Exito(true);
        }

        public async Task<Resultado<ExtensionDTO>> SolicitarExtension(int idPrestamo, CrearExtensionDTO dto)
        {
            var prestamo = await _repositorio.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                return Resultado<ExtensionDTO>.Falla($"No se encontró el préstamo con ID {idPrestamo}.");

            if (prestamo.IdEstado != ESTADO_APROBADO)
                return Resultado<ExtensionDTO>.Falla("Solo se puede solicitar extensión en préstamos aprobados.");

            if (dto.FechaExtensionSolicitada <= prestamo.FechaFinal)
                return Resultado<ExtensionDTO>.Falla("La fecha de extensión debe ser posterior a la fecha final actual.");

            var extensionesPrevias = await _repositorio.ObtenerExtensionsPorPrestamo(idPrestamo);
            if (extensionesPrevias.Any(e => e.IdEstado == ESTADO_PENDIENTE))
                return Resultado<ExtensionDTO>.Falla("Ya existe una extensión pendiente para este préstamo.");

            prestamo.IdEstado = ESTADO_EXTENSION_SOLICITADA;
            await _repositorio.Actualizar(prestamo);

            var nuevaExtension = new ExtensionPrestamosEquipo
            {
                IdPrestamos = idPrestamo,
                FechaExtensionSolicitada = dto.FechaExtensionSolicitada,
                FechaSolicitud = DateTime.Now,
                IdEstado = ESTADO_PENDIENTE,
                Motivo = dto.Motivo
            };

            await _repositorio.CrearExtension(nuevaExtension);
            await _repositorio.GuardarCambios();

            var admins = await _repositorio.ObtenerAdmins();
            var inventario = await _repositorio.ObtenerInventarioPorId(prestamo.IdInventario);
            foreach (var admin in admins)
                await _servicioEmail.EnviarCorreoReservaEquipos(
                    admin.CorreoInstitucional,
                    inventario?.Nombre ?? "Equipo",
                    prestamo.Cantidad.ToString(),
                    prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                    dto.FechaExtensionSolicitada.ToString("dd/MM/yyyy h:mm tt"));

            return Resultado<ExtensionDTO>.Exito(MapearExtensionDTO(nuevaExtension));
        }

        public async Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPendientes()
        {
            var lista = await _repositorio.ObtenerExtensionsPendientes();
            return Resultado<List<ExtensionDTO>>.Exito(lista.Select(MapearExtensionDTO).ToList());
        }

        public async Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPorPrestamo(int idPrestamo)
        {
            var lista = await _repositorio.ObtenerExtensionsPorPrestamo(idPrestamo);
            return Resultado<List<ExtensionDTO>>.Exito(lista.Select(MapearExtensionDTO).ToList());
        }

        // ── Privados ──────────────────────────────────────────────────────────────

        private async Task<Resultado<PrestamosEquipoDTO>> ProcesarSolicitud(AprobarRechazarSolicitudDTO dto)
        {
            var solicitud = await _context.SolicitudPrestamosDeEquipos
                .FirstOrDefaultAsync(s => s.Id == dto.IdSolicitud);

            if (solicitud == null)
                return Resultado<PrestamosEquipoDTO>.Falla($"No se encontró la solicitud con ID {dto.IdSolicitud}.");

            if (solicitud.IdEstado != ESTADO_PENDIENTE)
                return Resultado<PrestamosEquipoDTO>.Falla("Solo se pueden procesar solicitudes en estado Pendiente.");

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                if (dto.Aprobado)
                {
                    var inventario = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (inventario == null) { await transaccion.RollbackAsync(); return Resultado<PrestamosEquipoDTO>.Falla("No se encontró el equipo en inventario."); }

                    int cantidadReservada = await _context.PrestamosEquipos
                        .Where(p => p.IdInventario == solicitud.IdInventario
                                 && p.IdEstado == ESTADO_APROBADO
                                 && p.FechaInicio < solicitud.FechaFinal
                                 && p.FechaFinal > solicitud.FechaInicio)
                        .SumAsync(p => p.Cantidad ?? 0);

                    int stockDisponible = (inventario.Cantidad ?? 0) - cantidadReservada;
                    if (solicitud.Cantidad > stockDisponible)
                    {
                        await transaccion.RollbackAsync();
                        return Resultado<PrestamosEquipoDTO>.Falla($"Stock insuficiente. Disponible: {stockDisponible}, Solicitado: {solicitud.Cantidad}.");
                    }

                    inventario.Cantidad -= solicitud.Cantidad ?? 0;
                    _context.InventarioEquipos.Update(inventario);

                    var nuevoPrestamo = new PrestamosEquipo
                    {
                        IdUsuario = solicitud.IdUsuario,
                        IdInventario = solicitud.IdInventario,
                        IdEstado = ESTADO_APROBADO,
                        FechaInicio = solicitud.FechaInicio,
                        FechaFinal = solicitud.FechaFinal,
                        Motivo = solicitud.Motivo,
                        Cantidad = solicitud.Cantidad,
                        IdUsuarioAprobador = dto.IdUsuarioAprobador,
                        ComentarioAprobacion = dto.ComentarioAprobacion,
                        Activado = true
                    };
                    _context.PrestamosEquipos.Add(nuevoPrestamo);

                    solicitud.IdEstado = ESTADO_APROBADO;
                    _context.SolicitudPrestamosDeEquipos.Update(solicitud);

                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();

                    var usuario = await _repositorio.ObtenerUsuarioPorId(solicitud.IdUsuario);
                    var inv = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (usuario != null && inv != null)
                        await _servicioEmail.EnviarCorreoAprobacionEquipos(
                            usuario.CorreoInstitucional, usuario.NombreUsuario,
                            solicitud.Cantidad.ToString(), usuario.ApellidoUsuario, inv.Nombre,
                            solicitud.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                            solicitud.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));

                    return Resultado<PrestamosEquipoDTO>.Exito(MapearDTO(nuevoPrestamo));
                }
                else
                {
                    solicitud.IdEstado = ESTADO_RECHAZADO;
                    _context.SolicitudPrestamosDeEquipos.Update(solicitud);
                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();

                    var usuario = await _repositorio.ObtenerUsuarioPorId(solicitud.IdUsuario);
                    var inv = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (usuario != null && inv != null)
                        await _servicioEmail.EnviarCorreoRechazoEquipos(
                            usuario.CorreoInstitucional, usuario.NombreUsuario,
                            solicitud.Cantidad.ToString(), usuario.ApellidoUsuario, inv.Nombre,
                            dto.ComentarioAprobacion,
                            solicitud.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                            solicitud.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));

                    return Resultado<PrestamosEquipoDTO>.Exito(null);
                }
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return Resultado<PrestamosEquipoDTO>.Falla($"Error al procesar la solicitud: {ex.Message}");
            }
        }

        private async Task<Resultado<ExtensionDTO>> AprobarRechazarExtension(int idExtension, AprobarRechazarExtensionDTO dto)
        {
            var extension = await _repositorio.ObtenerExtensionPorId(idExtension);
            if (extension == null)
                return Resultado<ExtensionDTO>.Falla($"No se encontró la extensión con ID {idExtension}.");

            if (extension.IdEstado != ESTADO_PENDIENTE)
                return Resultado<ExtensionDTO>.Falla("Esta extensión ya fue procesada.");

            var prestamo = await _repositorio.ObtenerPorId(extension.IdPrestamos);
            if (prestamo == null)
                return Resultado<ExtensionDTO>.Falla("No se encontró el préstamo asociado.");

            extension.IdEstado = dto.Aprobado ? ESTADO_APROBADO : ESTADO_RECHAZADO;
            extension.IdUsuarioAprobador = dto.IdUsuarioAprobador;
            extension.ComentarioAprobacion = dto.ComentarioAprobacion;

            if (dto.Aprobado)
                prestamo.FechaFinal = extension.FechaExtensionSolicitada;

            prestamo.IdEstado = ESTADO_APROBADO;
            prestamo.IdUsuarioAprobador = dto.IdUsuarioAprobador;

            await _repositorio.ActualizarExtension(extension);
            await _repositorio.Actualizar(prestamo);
            await _repositorio.GuardarCambios();

            var usuario = await _repositorio.ObtenerUsuarioPorId(prestamo.IdUsuario);
            var inventario = await _repositorio.ObtenerInventarioPorId(prestamo.IdInventario);

            if (usuario != null && inventario != null)
            {
                if (dto.Aprobado)
                    await _servicioEmail.EnviarCorreoAprobacionEquipos(
                        usuario.CorreoInstitucional, usuario.NombreUsuario,
                        prestamo.Cantidad.ToString(), usuario.ApellidoUsuario, inventario.Nombre,
                        prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                        extension.FechaExtensionSolicitada.ToString("dd/MM/yyyy h:mm tt"));
                else
                    await _servicioEmail.EnviarCorreoRechazoEquipos(
                        usuario.CorreoInstitucional, usuario.NombreUsuario,
                        prestamo.Cantidad.ToString(), usuario.ApellidoUsuario, inventario.Nombre,
                        dto.ComentarioAprobacion,
                        prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                        prestamo.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));
            }

            return Resultado<ExtensionDTO>.Exito(MapearExtensionDTO(extension));
        }

        private static PrestamosEquipoDTO CalcularAtraso(PrestamosEquipoDTO dto)
        {
            if (dto.IdEstado == ESTADO_APROBADO &&
                dto.FechaFinal < DateTime.Now &&
                dto.FechaEntrega == null)
            {
                dto.EstaAtrasado = true;
                dto.DiasAtraso = (int)(DateTime.Now - dto.FechaFinal).TotalDays;
            }
            return dto;
        }

        private static PrestamosEquipoDTO MapearDTO(PrestamosEquipo p) => new()
        {
            Id = p.Id,
            IdUsuario = p.IdUsuario,
            IdInventario = p.IdInventario,
            IdEstado = p.IdEstado,
            FechaInicio = p.FechaInicio,
            FechaFinal = p.FechaFinal,
            FechaEntrega = p.FechaEntrega,
            IdUsuarioAprobador = p.IdUsuarioAprobador,
            Motivo = p.Motivo,
            ComentarioAprobacion = p.ComentarioAprobacion,
            Activado = p.Activado,
            Cantidad = p.Cantidad
        };

        private static ExtensionDTO MapearExtensionDTO(ExtensionPrestamosEquipo e) => new()
        {
            Id = e.Id,
            IdPrestamo = e.IdPrestamos,
            FechaExtensionSolicitada = e.FechaExtensionSolicitada,
            FechaSolicitud = e.FechaSolicitud,
            IdEstado = e.IdEstado,
            Motivo = e.Motivo,
            ComentarioAprobacion = e.ComentarioAprobacion,
            IdUsuarioAprobador = e.IdUsuarioAprobador
        };
    }
}