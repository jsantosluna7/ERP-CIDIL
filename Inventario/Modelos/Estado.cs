using System;
using System.Collections.Generic;

namespace Inventario.Modelos;

public partial class Estado
{
    public int Id { get; set; }

    public string Estado1 { get; set; } = null!;

    public virtual ICollection<PrestamosEquipo> PrestamosEquipos { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<ReservaDeEspacio> ReservaDeEspacios { get; set; } = new List<ReservaDeEspacio>();

    public virtual ICollection<SolicitudPrestamosDeEquipo> SolicitudPrestamosDeEquipos { get; set; } = new List<SolicitudPrestamosDeEquipo>();

    public virtual ICollection<SolicitudReservaDeEspacio> SolicitudReservaDeEspacios { get; set; } = new List<SolicitudReservaDeEspacio>();
}
