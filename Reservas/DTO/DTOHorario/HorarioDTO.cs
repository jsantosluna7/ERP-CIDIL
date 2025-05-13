namespace Reservas.DTO.DTOHorario
{
    public class HorarioDTO
    {
        public int Id { get; set; }

        public string Asignatura { get; set; } = null!;

        public string Profesor { get; set; } = null!;

        public int? IdLaboratorio { get; set; }

        public TimeOnly? HoraInicio { get; set; }

        public TimeOnly? HoraFinal { get; set; }

        public string Dia { get; set; } = null!;
    }
}
