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

    public string? Moneda { get; set; }

    public decimal? ImporteTotal { get; set; }

    public string? Comentario { get; set; }

    public int? EstadoTimelineId { get; set; }

    public int? CreadoPor { get; set; }

    public DateTime? ActualizadoEn { get; set; }

    [JsonIgnore]
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual EstadosTimeline? EstadoTimeline { get; set; }
    [JsonIgnore]
    public virtual ICollection<OrdenItem> OrdenItems { get; set; } = new List<OrdenItem>();
    [JsonIgnore]
    public virtual ICollection<OrdenTimeline> OrdenTimelines { get; set; } = new List<OrdenTimeline>();
}
