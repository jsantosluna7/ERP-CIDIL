using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    public DateTime? HoraInicio { get; set; }

    public DateTime? HoraFinal { get; set; }
    
    public virtual Laboratorio? IdLaboratorioNavigation { get; set; }
}
