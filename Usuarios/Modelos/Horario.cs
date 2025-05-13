using System;
using System.Collections.Generic;

namespace Usuarios.Modelos;

public partial class Horario
{
    public int Id { get; set; }

    public string? Asignatura { get; set; }

    public string? Profesor { get; set; }

    public int? IdLaboratorio { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFinal { get; set; }

    public string? Dia { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Laboratorio? IdLaboratorioNavigation { get; set; }
}
