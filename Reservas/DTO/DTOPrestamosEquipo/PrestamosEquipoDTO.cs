namespace Reservas.DTO.DTOPrestamosEquipo
{
    public class PrestamosEquipoDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdInventario { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int? IdUsuarioAprobador { get; set; }
        public string Motivo { get; set; } = null!;
        public string? ComentarioAprobacion { get; set; }
        public bool? Activado { get; set; }
        public int? Cantidad { get; set; }
        // Calculado en el servicio
        public bool EstaAtrasado { get; set; }
        public int? DiasAtraso { get; set; }
    }

    public class CrearPrestamosEquipoDTO
    {
        public int IdUsuario { get; set; }
        public int IdInventario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Motivo { get; set; } = null!;
        public int? Cantidad { get; set; }
    }

    public class AprobarRechazarSolicitudDTO
    {
        public int IdSolicitud { get; set; }
        public int IdUsuarioAprobador { get; set; }
        public bool Aprobado { get; set; }
        public string? ComentarioAprobacion { get; set; }
    }

    public class MarcarDevueltoDTO
    {
        public DateTime FechaEntrega { get; set; }
    }

    // DTOs para extensiones
    public class CrearExtensionDTO
    {
        public DateTime FechaExtensionSolicitada { get; set; }
        public string? Motivo { get; set; }
    }

    public class AprobarRechazarExtensionDTO
    {
        public int IdUsuarioAprobador { get; set; }
        public bool Aprobado { get; set; }
        public string? ComentarioAprobacion { get; set; }
    }

    public class ExtensionDTO
    {
        public int Id { get; set; }
        public int IdPrestamo { get; set; }
        public DateTime FechaExtensionSolicitada { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
        public string? ComentarioAprobacion { get; set; }
        public int? IdUsuarioAprobador { get; set; }
    }

    public class ResumenPrestamosDTO
    {
        public int TotalPendientes { get; set; }
        public int TotalActivos { get; set; }
        public int TotalAtrasados { get; set; }
        public int TotalExtensionsSolicitadas { get; set; }
    }
}
