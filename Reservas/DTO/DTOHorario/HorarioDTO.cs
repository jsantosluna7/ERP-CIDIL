namespace Reservas.DTO.DTOHorario
{
    public class HorarioDTO
    {
        public int Id { get; set; }

        public string? Asignatura { get; set; }

        public string? Profesor { get; set; }

        public int? IdLaboratorio { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public TimeSpan? HoraFinal { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public string? Dia { get; set; }
    }
}
