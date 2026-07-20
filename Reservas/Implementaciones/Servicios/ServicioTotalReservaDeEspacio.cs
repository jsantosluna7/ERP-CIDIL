using ERP.Data.Modelos;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOTotalReservaEspacios;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioTotalReservaDeEspacio : IServicioTotalReservaDeEspacio
    {
        private readonly IRepositorioTotalReservaDeEspacio _repositorio;

        public ServicioTotalReservaDeEspacio(IRepositorioTotalReservaDeEspacio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Resultado<List<ReservaDeEspacioUsuarioDTO>>> ObtenerTodasLasReservasDeEspacioDelUsuario(
            int idUsuario,
            int? idEstado = null)
        {
            // ── Validación ───────────────────────────────────────────────────────
            var estadosPermitidos = new[] { 1, 2, 3 };
            if (idEstado.HasValue && !estadosPermitidos.Contains(idEstado.Value))
            {
                return Resultado<List<ReservaDeEspacioUsuarioDTO>>.Falla(
                    $"El estado '{idEstado}' no es válido. Use: 1 (Aprobado), 2 (Pendiente), 3 (Rechazado).");
            }

            // ── 1. Consultas ─────────────────────────────────────────────────────
            var solicitudes = (idEstado == null || idEstado == 2)
                ? await _repositorio.ObtenerSolicitudesPendientesPorUsuario(idUsuario)
                : new List<ReservaDeEspacioUsuarioDTO>();

            var reservas = (idEstado == null || idEstado == 1 || idEstado == 3)
                ? await _repositorio.ObtenerReservasResueltasPorUsuario(idUsuario)
                : new List<ReservaDeEspacioUsuarioDTO>();

            if (solicitudes.Count == 0 && reservas.Count == 0)
            {
                return Resultado<List<ReservaDeEspacioUsuarioDTO>>.Falla(
                    "No se encontraron reservas de espacio para el usuario especificado.");
            }

            // ── 2. Obtener laboratorios ──────────────────────────────────────────
            var todos = solicitudes.Concat(reservas).ToList();

            var idsLaboratorios = todos
                .Select(x => x.IdLaboratorio)
                .Distinct()
                .ToList();

            var laboratorios = await _repositorio.ObtenerLaboratoriosPorIds(idsLaboratorios);
            var dicLaboratorios = laboratorios.ToDictionary(l => l.Id);

            // ── 3. Mapear info de laboratorio ────────────────────────────────────
            foreach (var item in todos)
            {
                if (dicLaboratorios.TryGetValue(item.IdLaboratorio, out var lab))
                {
                    item.NombreEspacio = lab.Nombre;
                    item.ImagenLaboratorio = lab.ImagenLaboratorio;
                }
                else
                {
                    item.NombreEspacio = "Desconocido";
                    item.ImagenLaboratorio = "";
                }
            }

            // ── 4. Filtrar, ordenar ──────────────────────────────────────────────
            var resultado = todos
                .Where(r => idEstado == null || r.IdEstado == idEstado)
                .OrderByDescending(x => x.FechaSolicitud)
                .ToList();

            return Resultado<List<ReservaDeEspacioUsuarioDTO>>.Exito(resultado);
        }

        public async Task<Resultado<List<ReservaEspaciosAdminDTO>>> ObtenerTodasLasReservasDeEspacios(
            int? idEstado = null,
            string? busqueda = null,
            int pagina = 1,
            int tamanoPagina = 20)
        {
            // ── Validaciones ──────────────────────────────────────────────────────
            if (pagina <= 0)
                return Resultado<List<ReservaEspaciosAdminDTO>>.Falla("El número de página debe ser mayor a 0.");

            if (tamanoPagina <= 0 || tamanoPagina > 100)
                return Resultado<List<ReservaEspaciosAdminDTO>>.Falla("El tamaño de página debe estar entre 1 y 100.");

            // ── 1. Traer estados desde BD y construir diccionario ─────────────────
            var estados = await _repositorio.ObtenerEstados();
            var dicEstados = estados.ToDictionary(e => e.Id, e => e.Estado1);

            // Validar que el idEstado exista en BD
            if (idEstado.HasValue && !dicEstados.ContainsKey(idEstado.Value))
                return Resultado<List<ReservaEspaciosAdminDTO>>.Falla(
                    $"El estado '{idEstado}' no existe. Estados válidos: {string.Join(", ", dicEstados.Select(e => $"{e.Key} ({e.Value})"))}.");

            // ── 2. Consultas a BD por separado ────────────────────────────────────
            var solicitudes = new List<ReservaEspaciosAdminDTO>();
            var reservas = new List<ReservaEspaciosAdminDTO>();

            // Pendientes: id de estado 2 (PENDIENTE)
            if (idEstado == null || idEstado == 2)
            {
                var datosSolicitudes = await _repositorio.ObtenerSolicitudesPendientes();

                if (datosSolicitudes.Count > 0)
                {
                    var idsUsuarios = datosSolicitudes.Select(s => s.IdUsuario).Distinct().ToList();
                    var idsLaboratorios = datosSolicitudes.Select(s => s.IdLaboratorio).Distinct().ToList();

                    var usuarios = await _repositorio.ObtenerUsuariosPorIds(idsUsuarios);
                    var laboratorios = await _repositorio.ObtenerLaboratoriosPorIds(idsLaboratorios);

                    var dicUsuarios = usuarios.ToDictionary(u => u.Id);
                    var dicLaboratorios = laboratorios.ToDictionary(l => l.Id);

                    solicitudes = datosSolicitudes
                        .Where(s => idEstado == null || s.IdEstado == idEstado)
                        .Select(s => new ReservaEspaciosAdminDTO
                        {
                            Id = s.Id,
                            IdUsuario = s.IdUsuario,
                            NombreSolicitante = dicUsuarios.TryGetValue(s.IdUsuario, out var u) ? u.NombreUsuario : "Desconocido",
                            ApellidoSolicitante = dicUsuarios.TryGetValue(s.IdUsuario, out var u2) ? u2.ApellidoUsuario : "",
                            IdLaboratorio = s.IdLaboratorio,
                            NombreEspacio = dicLaboratorios.TryGetValue(s.IdLaboratorio, out var l) ? l.Nombre : "Desconocido",
                            Motivo = s.Motivo,
                            FechaSolicitud = s.FechaSolicitud,
                            IdEstado = s.IdEstado,
                            NombreEstado = s.IdEstado.HasValue && dicEstados.TryGetValue(s.IdEstado.Value, out var eS) ? eS : "Desconocido",
                            TipoRegistro = "Solicitud",
                            HoraInicio = s.HoraInicio,
                            HoraFinal = s.HoraFinal,
                            FechaInicio = s.FechaInicio,
                            FechaFinal = s.FechaFinal,
                            PersonasCantidad = s.PersonasCantidad,
                            ImagenLaboratorio = dicLaboratorios.TryGetValue(s.IdLaboratorio, out var l2) ? l2.ImagenLaboratorio : ""
                        }).ToList();
                }
            }

            // Resueltas: id de estado 1 (APROBADO) o 3 (RECHAZADO)
            if (idEstado == null || idEstado == 1 || idEstado == 3)
            {
                var datosReservas = await _repositorio.ObtenerReservasResueltas();

                if (datosReservas.Count > 0)
                {
                    var idsUsuarios = datosReservas.Select(r => r.IdUsuario).Distinct().ToList();
                    var idsAprobadores = datosReservas.Where(r => r.IdUsuarioAprobador.HasValue)
                                                      .Select(r => r.IdUsuarioAprobador!.Value).Distinct().ToList();
                    var idsLaboratorios = datosReservas.Select(r => r.IdLaboratorio).Distinct().ToList();

                    // Solicitantes y aprobadores en una sola consulta
                    var todosIdsUsuarios = idsUsuarios.Union(idsAprobadores).Distinct().ToList();

                    var usuarios = await _repositorio.ObtenerUsuariosPorIds(todosIdsUsuarios);
                    var laboratorios = await _repositorio.ObtenerLaboratoriosPorIds(idsLaboratorios);

                    var dicUsuarios = usuarios.ToDictionary(u => u.Id);
                    var dicLaboratorios = laboratorios.ToDictionary(l => l.Id);

                    reservas = datosReservas
                        .Where(r => idEstado == null || r.IdEstado == idEstado)
                        .Select(r => new ReservaEspaciosAdminDTO
                        {
                            Id = r.Id,
                            IdUsuario = r.IdUsuario,
                            NombreSolicitante = dicUsuarios.TryGetValue(r.IdUsuario, out var u) ? u.NombreUsuario : "Desconocido",
                            ApellidoSolicitante = dicUsuarios.TryGetValue(r.IdUsuario, out var u2) ? u2.ApellidoUsuario : "",
                            IdLaboratorio = r.IdLaboratorio,
                            NombreEspacio = dicLaboratorios.TryGetValue(r.IdLaboratorio, out var l) ? l.Nombre : "Desconocido",
                            Motivo = r.Motivo,
                            FechaSolicitud = r.FechaSolicitud,
                            IdEstado = r.IdEstado,
                            NombreEstado = dicEstados.TryGetValue(r.IdEstado, out var eR) ? eR : "Desconocido",
                            TipoRegistro = "Reserva",
                            HoraInicio = r.HoraInicio,
                            HoraFinal = r.HoraFinal,
                            FechaInicio = r.FechaInicio ?? default,
                            FechaFinal = r.FechaFinal ?? default,
                            PersonasCantidad = r.PersonasCantidad,
                            IdUsuarioAprobador = r.IdUsuarioAprobador,
                            NombreAprobador = r.IdUsuarioAprobador.HasValue && dicUsuarios.TryGetValue(r.IdUsuarioAprobador.Value, out var ap)
                                                    ? $"{ap.NombreUsuario} {ap.ApellidoUsuario}" : null,
                            FechaAprobacion = r.FechaAprobacion,
                            ComentarioAprobacion = r.ComentarioAprobacion,
                            ImagenLaboratorio = dicLaboratorios.TryGetValue(r.IdLaboratorio, out var l2) ? l2.ImagenLaboratorio : "",
                        }).ToList();
                }
            }

            // ── 3. Combinar ───────────────────────────────────────────────────────
            var todas = solicitudes.Concat(reservas).ToList();

            if (todas.Count == 0)
                return Resultado<List<ReservaEspaciosAdminDTO>>.Falla(
                    "No se encontraron reservas de espacios con los filtros aplicados.");

            // ── 4. Filtro por búsqueda ────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                var busquedaNorm = busqueda.Trim().ToLower();
                todas = todas.Where(x =>
                    x.NombreSolicitante.ToLower().Contains(busquedaNorm) ||
                    x.ApellidoSolicitante.ToLower().Contains(busquedaNorm) ||
                    x.NombreEspacio.ToLower().Contains(busquedaNorm))
                    .ToList();

                if (todas.Count == 0)
                    return Resultado<List<ReservaEspaciosAdminDTO>>.Falla(
                        $"No se encontraron resultados para la búsqueda '{busqueda}'.");
            }

            // ── 5. Ordenar y paginar ──────────────────────────────────────────────
            var paginado = todas
                .OrderByDescending(x => x.FechaSolicitud)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return Resultado<List<ReservaEspaciosAdminDTO>>.Exito(paginado);
        }

        public async Task<Resultado<ConteoReservaEspaciosDTO>> ObtenerConteoReservasDeEspacios()
        {
            var total = await _repositorio.ContarTotalSolicitudes();
            var pendientes = await _repositorio.ContarSolicitudesPendientes();
            var aprobadas = await _repositorio.ContarSolicitudesAprobadas();
            var rechazadas = await _repositorio.ContarSolicitudesRechazadas();

            var conteo = new ConteoReservaEspaciosDTO
            {
                TotalSolicitudes = total,
                TotalPendientes = pendientes,
                TotalAprobadas = aprobadas,
                TotalRechazadas = rechazadas
            };

            return Resultado<ConteoReservaEspaciosDTO>.Exito(conteo);
        }
    }
}
