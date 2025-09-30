namespace Usuarios.DTO.ReporteFallaDTO
{
    public class CrearReporteFallaDTO
    {
        public string Descripcion { get; set; } = null!;

        public string? Lugar { get; set; }

        public int Estado { get; set; }

        public int IdUsuario { get; set; }
    }
}
