using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class EstadosTimeline
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Color { get; set; }

    public string? Icono { get; set; }

    public bool? Activo { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();
    [JsonIgnore]
    public virtual ICollection<OrdenTimeline> OrdenTimelines { get; set; } = new List<OrdenTimeline>();
    [JsonIgnore]
    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();
}
