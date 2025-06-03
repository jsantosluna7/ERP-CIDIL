namespace Reservas.DTO.DTOHorario
{
    public class CrearHorarioDTO
    {

        public string? Asignatura { get; set; } = null!;

        public string? Profesor { get; set; } = null!;

        public int? IdLaboratorio { get; set; }

        public DateTime? HoraInicio { get; set; }

        public DateTime? HoraFinal { get; set; }

        public string? Dia { get; set; } = null!;
    }
}
