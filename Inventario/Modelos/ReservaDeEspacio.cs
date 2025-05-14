using System;
using System.Collections.Generic;

namespace Inventario.Modelos;

public partial class ReservaDeEspacio
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdLaboratorio { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFinal { get; set; }

    public int IdEstado { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdUsuarioAprobador { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string? ComentarioAprobacion { get; set; }

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioAprobadorNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
