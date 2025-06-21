using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Horario
{
    public int Id { get; set; }

    public string? Asignatura { get; set; }

    public string? Profesor { get; set; }

    public int? IdLaboratorio { get; set; }

    public string? Dia { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? ActivadoHorario { get; set; }

    public TimeSpan? HoraInicio { get; set; }

    public TimeSpan? HoraFinal { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinal { get; set; }

    public virtual Laboratorio? IdLaboratorioNavigation { get; set; }
}
