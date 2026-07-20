namespace Usuarios.DTO.ReporteFallaDTO
{
    public class ReporteFallaDTO
    {
        public int IdReporte { get; set; }

        public string Descripcion { get; set; } = null!;

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaUltimaActualizacion { get; set; }

        public string? Lugar { get; set; }

        public int Estado { get; set; }

        public int IdUsuario { get; set; }

    }
}
