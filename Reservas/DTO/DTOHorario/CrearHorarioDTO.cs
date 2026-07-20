public class CrearHorarioDTO
{
    public string? Asignatura { get; set; }
    public string? Profesor { get; set; }
    public int? IdLaboratorio { get; set; }
    public string? Dia { get; set; }
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFinal { get; set; } = string.Empty;
    public DateTimeOffset FechaInicio { get; set; }
    public DateTimeOffset FechaFinal { get; set; }
}
