namespace Reservas.DTO.DTOSolicitudDeEquipos
{
    public class SolicitudPrestamosDeEquiposDTO
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public int IdInventario { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public string Motivo { get; set; } = null!;

        public DateTime? FechaSolicitud { get; set; }

        
    }
}
