using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class PrestamosEquipo
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdInventario { get; set; }

    public int IdEstado { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinal { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public int? IdUsuarioAprobador { get; set; }

    public string Motivo { get; set; } = null!;

    public string? ComentarioAprobacion { get; set; }

    public bool? Activado { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAprobadorNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
