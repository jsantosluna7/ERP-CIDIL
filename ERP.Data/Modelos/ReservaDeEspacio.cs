using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class ReservaDeEspacio
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdLaboratorio { get; set; }

    public int IdEstado { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdUsuarioAprobador { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string? ComentarioAprobacion { get; set; }

    public bool? Activado { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFinal { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinal { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAprobadorNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
