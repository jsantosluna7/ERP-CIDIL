using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class Ordene
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Departamento { get; set; }

    public string? UnidadNegocio { get; set; }

    public string? SolicitadoPor { get; set; }

    public DateOnly? FechaSolicitud { get; set; }

    public DateOnly? FechaSubida { get; set; }

    public int? ItemsCount { get; set; }

    public string? Comentario { get; set; }

    public int? EstadoTimelineId { get; set; }

    public int? CreadoPor { get; set; }

    public DateTime? ActualizadoEn { get; set; }
    [JsonIgnore]
    public virtual ICollection<ComentariosOrden> ComentariosOrdens { get; set; } = new List<ComentariosOrden>();
    [JsonIgnore]
    public virtual Usuario? CreadoPorNavigation { get; set; }
    [JsonIgnore]
    public virtual EstadosTimeline? EstadoTimeline { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();
    [JsonIgnore]
    public virtual ICollection<OrdenTimeline> OrdenTimelines { get; set; } = new List<OrdenTimeline>();
}
