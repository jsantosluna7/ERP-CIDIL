using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class EstadosTimeline
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Color { get; set; }

    public string? Icono { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();

    public virtual ICollection<OrdenTimeline> OrdenTimelines { get; set; } = new List<OrdenTimeline>();

    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();
}
