using System;
using System.Collections.Generic;

namespace Inventario.Modelos;

public partial class Estado
{
    public int Id { get; set; }

    public string Estado1 { get; set; } = null!;

    public virtual ICollection<PrestamosEquipo> PrestamosEquipos { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<PrestamosEspacio> PrestamosEspacios { get; set; } = new List<PrestamosEspacio>();
}
