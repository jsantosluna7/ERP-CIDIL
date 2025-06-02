namespace Reservas.DTO.DTOSolicitudDeReserva
{
    public class ActualizarSolicitudDeReservaDTO
    {
        public int? IdLaboratorio { get; set; }

        public DateTime? HoraInicio { get; set; }
                       
        public DateTime? HoraFinal { get; set; }

        public string? Motivo { get; set; } = null!;

        public DateTime? FechaSolicitud { get; set; }
        //public int? IdEstado { get; set; }
    }
}
