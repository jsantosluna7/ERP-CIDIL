namespace Reservas.DTO.DTOReservaDeEspacio
{
    public class ActualizarReservaDeEspacioDTO
    {
        public int? IdUsuario { get; set; }

        public int? IdLaboratorio { get; set; }

        public TimeOnly? HoraInicio { get; set; }

        public TimeOnly? HoraFinal { get; set; }

        public int? IdEstado { get; set; }

        public string? Motivo { get; set; } = null!;

        public DateTime? FechaSolicitud { get; set; }

        public int? IdUsuarioAprobador { get; set; }

        public DateTime? FechaAprobacion { get; set; }

        public string? ComentarioAprobacion { get; set; }

    }
}
