using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class OrdenItem
{
    public int Id { get; set; }

    public int OrdenId { get; set; }

    public string? NumeroLista { get; set; }

    public string Nombre { get; set; } = null!;

    public int? EstadoTimelineId { get; set; }

    public int Cantidad { get; set; }

    public int? CantidadRecibida { get; set; }

    public string? Comentario { get; set; }

    public DateTime? ActualizadoEn { get; set; }
    [JsonIgnore]
    public virtual ICollection<ComentariosOrden> ComentariosOrdens { get; set; } = new List<ComentariosOrden>();
    [JsonIgnore]
    public virtual EstadosTimeline? EstadoTimeline { get; set; }
    [JsonIgnore]
    public virtual Ordene Orden { get; set; } = null!;
}
