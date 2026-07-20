namespace Reservas.DTO.DTOTotalReservaEspacios
{
    public class ReservaDeEspacioUsuarioDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdLaboratorio { get; set; }
        public string Motivo { get; set; } = null!;
        public DateTime? FechaSolicitud { get; set; }
        public int? IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;
        public string TipoRegistro { get; set; } = null!;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int PersonasCantidad { get; set; }
        public int? IdUsuarioAprobador { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? ComentarioAprobacion { get; set; }
        public string? ImagenLaboratorio { get; set; }
        public string? NombreEspacio { get; set; }
    }

    public class ReservaEspaciosAdminDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string NombreSolicitante { get; set; } = null!;
        public string ApellidoSolicitante { get; set; } = null!;
        public int IdLaboratorio { get; set; }
        public string NombreEspacio { get; set; } = null!;
        public string? ImagenLaboratorio { get; set; }
        public string Motivo { get; set; } = null!;
        public DateTime? FechaSolicitud { get; set; }
        public int? IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;
        public string TipoRegistro { get; set; } = null!;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int PersonasCantidad { get; set; }
        public int? IdUsuarioAprobador { get; set; }
        public string? NombreAprobador { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? ComentarioAprobacion { get; set; }
    }

    public class ConteoReservaEspaciosDTO
    {
        public int TotalSolicitudes { get; set; }
        public int TotalPendientes { get; set; }
        public int TotalAprobadas { get; set; }
        public int TotalRechazadas { get; set; }
    }
}
