using System;
using System.Collections.Generic;

namespace Reservas.Modelos;

public partial class Horario
{
    public int Id { get; set; }

    public string Asignatura { get; set; } = null!;

    public string Profesor { get; set; } = null!;

    public int IdLaboratorio { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFinal { get; set; }

    public string Dia { get; set; } = null!;

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;
}
