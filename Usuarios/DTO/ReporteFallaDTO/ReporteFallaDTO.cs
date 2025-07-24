namespace Usuarios.DTO.ReporteFallaDTO
{
    public class ReporteFallaDTO
    {
        public int IdReporte { get; set; }

        public int? IdLaboratorio { get; set; }

        public string Descripcion { get; set; } = null!;

        public string NombreSolicitante { get; set; } = null!;

        public int IdEstado { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaUltimaActualizacion { get; set; }
        public string? Lugar { get; set; }
    }
}
