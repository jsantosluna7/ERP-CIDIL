namespace Reservas.DTO.DTOSolicitudDeReserva
{
    public class ActualizarSolicitudDeReservaDTO
    {
        public int? IdLaboratorio { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFinal { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public string? Motivo { get; set; } = null!;

        public DateTime? FechaSolicitud { get; set; }
        //public int? IdEstado { get; set; }
    }
}
