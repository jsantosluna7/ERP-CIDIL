using System;
using System.Collections.Generic;

namespace Usuarios.Modelos;

public partial class SolicitudPrestamosDeEquipo
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdInventario { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFinal { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public int? IdEstado { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual InventarioEquipo IdInventarioNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
