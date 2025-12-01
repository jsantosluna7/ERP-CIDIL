using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class OrdenTimeline
{
    public int Id { get; set; }

    public int OrdenId { get; set; }

    public int? EstadoTimelineId { get; set; }

    public string Evento { get; set; } = null!;

    public DateTime? FechaEvento { get; set; }

    public int? CreadoPor { get; set; }

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual EstadosTimeline? EstadoTimeline { get; set; }

    public virtual Ordene Orden { get; set; } = null!;
}
