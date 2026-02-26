using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Implementaciones.Repositorios;

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

        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerTodos(int pagina, int tamanoPagina)
        {
            var lista = await _repositorio.ObtenerTodos(pagina, tamanoPagina);
            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
        }

        public async Task<Resultado<PrestamosEquipoDTO>> ObtenerPorId(int id)
        {
            var prestamo = await _repositorio.ObtenerPorId(id);
            if (prestamo == null)
                return Resultado<PrestamosEquipoDTO>.Falla($"No se encontró el préstamo con ID {id}.");

            var dto = MapearDTO(prestamo);
            return Resultado<PrestamosEquipoDTO>.Exito(CalcularAtraso(dto));
        }

        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPorUsuario(int idUsuario)
        {
            var lista = await _repositorio.ObtenerPorUsuario(idUsuario);
            if (!lista.Any())
                return Resultado<List<PrestamosEquipoDTO>>.Falla("No se encontraron préstamos para este usuario.");

            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
        }

        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPendientes()
        {
            var lista = await _repositorio.ObtenerPendientes();
            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista);
        }

        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerActivos()
        {
            var lista = await _repositorio.ObtenerActivos();
            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
        }

        public async Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerAtrasados()
        {
            var lista = await _repositorio.ObtenerAtrasados();
            return Resultado<List<PrestamosEquipoDTO>>.Exito(lista.Select(CalcularAtraso).ToList());
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

        /// <summary>
        /// El admin aprueba o rechaza una solicitud con un botón.
        /// Si aprueba → IdEstado = 1 (APROBADO)
        /// Si rechaza → IdEstado = 3 (RECHAZADO)
        /// </summary>

        // En ServicioPrestamosEquipo.cs
        public async Task<Resultado<PrestamosEquipoDTO>> ProcesarSolicitud(AprobarRechazarSolicitudDTO dto)
        {
            // 1. Obtener la solicitud con el id que manda el front
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
                    // 2. Verificar stock al momento de aprobar
                    var inventario = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (inventario == null)
                    {
                        await transaccion.RollbackAsync();
                        return Resultado<PrestamosEquipoDTO>.Falla("No se encontró el equipo en inventario.");
                    }

                    int cantidadYaReservada = await _context.PrestamosEquipos
                        .Where(p => p.IdInventario == solicitud.IdInventario
                                    && p.IdEstado == ESTADO_APROBADO
                                    && p.FechaInicio < solicitud.FechaFinal
                                    && p.FechaFinal > solicitud.FechaInicio)
                        .SumAsync(p => p.Cantidad ?? 0);

                    int stockDisponible = (inventario.Cantidad ?? 0) - cantidadYaReservada;

                    if (solicitud.Cantidad > stockDisponible)
                    {
                        await transaccion.RollbackAsync();
                        return Resultado<PrestamosEquipoDTO>.Falla(
                            $"Stock insuficiente. Disponible: {stockDisponible}, Solicitado: {solicitud.Cantidad}.");
                    }

                    // 3. Descontar inventario
                    inventario.Cantidad -= solicitud.Cantidad ?? 0;
                    _context.InventarioEquipos.Update(inventario);

                    // 4. Crear el préstamo con toda la info de la solicitud
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

                    // 5. Actualizar estado de la solicitud
                    solicitud.IdEstado = ESTADO_APROBADO;
                    _context.SolicitudPrestamosDeEquipos.Update(solicitud);

                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();

                    // 6. Notificar al usuario
                    var usuario = await _repositorio.ObtenerUsuarioPorId(solicitud.IdUsuario);
                    var inv = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (usuario != null && inv != null)
                        await _servicioEmail.EnviarCorreoAprobacionEquipos(
                            usuario.CorreoInstitucional,
                            usuario.NombreUsuario,
                            solicitud.Cantidad.ToString(),
                            usuario.ApellidoUsuario,
                            inv.Nombre,
                            solicitud.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                            solicitud.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));

                    return Resultado<PrestamosEquipoDTO>.Exito(MapearDTO(nuevoPrestamo));
                }
                else
                {
                    // Solo rechazar — no toca inventario ni crea préstamo
                    solicitud.IdEstado = ESTADO_RECHAZADO;
                    _context.SolicitudPrestamosDeEquipos.Update(solicitud);

                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();

                    // Notificar al usuario
                    var usuario = await _repositorio.ObtenerUsuarioPorId(solicitud.IdUsuario);
                    var inv = await _repositorio.ObtenerInventarioPorId(solicitud.IdInventario);
                    if (usuario != null && inv != null)
                        await _servicioEmail.EnviarCorreoRechazoEquipos(
                            usuario.CorreoInstitucional,
                            usuario.NombreUsuario,
                            solicitud.Cantidad.ToString(),
                            usuario.ApellidoUsuario,
                            inv.Nombre,
                            dto.ComentarioAprobacion,
                            solicitud.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                            solicitud.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));

                    // Retornamos null en Valor porque no se creó préstamo
                    return Resultado<PrestamosEquipoDTO>.Exito(null);
                }
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return Resultado<PrestamosEquipoDTO>.Falla($"Error al procesar la solicitud: {ex.Message}");
            }
        }

        /// <summary>
        /// Marca el préstamo como devuelto. Registra la fecha de entrega real
        /// y cambia el estado a DEVUELTO (4).
        /// </summary>
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

        // ─── Extensiones ──────────────────────────────────────────────────────────

        /// <summary>
        /// El usuario solicita una extensión. El préstamo pasa a estado
        /// EXTENSIÓN SOLICITADA (5) y se notifica a los admins.
        /// </summary>
        public async Task<Resultado<ExtensionDTO>> SolicitarExtension(int idPrestamo, CrearExtensionDTO dto)
        {
            var prestamo = await _repositorio.ObtenerPorId(idPrestamo);
            if (prestamo == null)
                return Resultado<ExtensionDTO>.Falla($"No se encontró el préstamo con ID {idPrestamo}.");

            if (prestamo.IdEstado != ESTADO_APROBADO)
                return Resultado<ExtensionDTO>.Falla("Solo se puede solicitar extensión en préstamos aprobados.");

            if (dto.FechaExtensionSolicitada <= prestamo.FechaFinal)
                return Resultado<ExtensionDTO>.Falla("La fecha de extensión debe ser posterior a la fecha final actual.");

            // Verificar que no haya ya una extensión pendiente para este préstamo
            var extensionesPrevias = await _repositorio.ObtenerExtensionsPorPrestamo(idPrestamo);
            bool tienePendiente = extensionesPrevias.Any(e => e.IdEstado == ESTADO_PENDIENTE);
            if (tienePendiente)
                return Resultado<ExtensionDTO>.Falla("Ya existe una extensión pendiente para este préstamo.");

            // Cambiar estado del préstamo a EXTENSIÓN SOLICITADA
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

            // Notificar admins
            var admins = await _repositorio.ObtenerAdmins();
            var inventario = await _repositorio.ObtenerInventarioPorId(prestamo.IdInventario);
            foreach (var admin in admins)
            {
                await _servicioEmail.EnviarCorreoReservaEquipos(
                    admin.CorreoInstitucional,
                    inventario?.Nombre ?? "Equipo",
                    prestamo.Cantidad.ToString(),
                    prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                    dto.FechaExtensionSolicitada.ToString("dd/MM/yyyy h:mm tt"));
            }

            return Resultado<ExtensionDTO>.Exito(MapearExtensionDTO(nuevaExtension));
        }

        /// <summary>
        /// El admin aprueba o rechaza la extensión.
        /// Si aprueba → actualiza FechaFinal del préstamo y vuelve a APROBADO.
        /// Si rechaza → vuelve a APROBADO sin cambiar fechas.
        /// </summary>
        public async Task<Resultado<ExtensionDTO>> AprobarRechazarExtension(int idExtension, AprobarRechazarExtensionDTO dto)
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
            {
                // Aplicar la nueva fecha final al préstamo
                prestamo.FechaFinal = extension.FechaExtensionSolicitada;
            }

            // El préstamo vuelve a APROBADO en ambos casos
            prestamo.IdEstado = ESTADO_APROBADO;
            prestamo.IdUsuarioAprobador = dto.IdUsuarioAprobador;

            await _repositorio.ActualizarExtension(extension);
            await _repositorio.Actualizar(prestamo);
            await _repositorio.GuardarCambios();

            // Notificar al usuario
            var usuario = await _repositorio.ObtenerUsuarioPorId(prestamo.IdUsuario);
            var inventario = await _repositorio.ObtenerInventarioPorId(prestamo.IdInventario);

            if (usuario != null && inventario != null)
            {
                if (dto.Aprobado)
                    await _servicioEmail.EnviarCorreoAprobacionEquipos(
                        usuario.CorreoInstitucional,
                        usuario.NombreUsuario,
                        prestamo.Cantidad.ToString(),
                        usuario.ApellidoUsuario,
                        inventario.Nombre,
                        prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                        extension.FechaExtensionSolicitada.ToString("dd/MM/yyyy h:mm tt"));
                else
                    await _servicioEmail.EnviarCorreoRechazoEquipos(
                        usuario.CorreoInstitucional,
                        usuario.NombreUsuario,
                        prestamo.Cantidad.ToString(),
                        usuario.ApellidoUsuario,
                        inventario.Nombre,
                        dto.ComentarioAprobacion,
                        prestamo.FechaInicio.ToString("dd/MM/yyyy h:mm tt"),
                        prestamo.FechaFinal.ToString("dd/MM/yyyy h:mm tt"));
            }

            return Resultado<ExtensionDTO>.Exito(MapearExtensionDTO(extension));
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

        // ─── Helpers ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Calcula si está atrasado y cuántos días lleva de atraso.
        /// Atrasado = Aprobado + FechaFinal pasó + no devuelto (FechaEntrega null)
        /// </summary>
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
            Cantidad = p.Cantidad,
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
            IdUsuarioAprobador = e.IdUsuarioAprobador,
        };
    }
}
