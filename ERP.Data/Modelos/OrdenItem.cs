using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class OrdenItem
{
    public int Id { get; set; }

    public int OrdenId { get; set; }

    public string? NumeroLista { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly? FechaSolicitud { get; set; }

    public DateOnly? FechaSubida { get; set; }

    public DateOnly? FechaRecibido { get; set; }

    public DateOnly? FechaEstimadaEntrega { get; set; }

    public int? EstadoTimelineId { get; set; }

    public int Cantidad { get; set; }

    public int? CantidadRecibida { get; set; }

    public string? UnidadMedida { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? ImporteLinea { get; set; }

    public string? LinkExterno { get; set; }

    public string? Comentario { get; set; }

    public string? DireccionEnvio { get; set; }

    public string? Atencion { get; set; }

    public string? EnvioVia { get; set; }

    public string? TerminosEnvio { get; set; }

    public DateTime? ActualizadoEn { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual EstadosTimeline? EstadoTimeline { get; set; }

    public virtual Ordene Orden { get; set; } = null!;
}
