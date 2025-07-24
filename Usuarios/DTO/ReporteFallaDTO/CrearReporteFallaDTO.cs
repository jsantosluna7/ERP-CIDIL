namespace Usuarios.DTO.ReporteFallaDTO
{
    public class CrearReporteFallaDTO
    {
        public int? IdLaboratorio { get; set; }

        public string Descripcion { get; set; } = null!;

        public string NombreSolicitante { get; set; } = null!;

        public int IdEstado { get; set; }
        public string? Lugar { get; set; }

    }
}
