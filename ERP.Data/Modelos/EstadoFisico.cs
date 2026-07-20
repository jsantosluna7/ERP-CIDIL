using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class EstadoFisico
{
    public int Id { get; set; }

    public string EstadoFisico1 { get; set; } = null!;

    public virtual ICollection<InventarioEquipo> InventarioEquipos { get; set; } = new List<InventarioEquipo>();
}
