using System;
using System.Collections.Generic;

namespace Inventario.Modelos;

public partial class Laboratorio
{
    public int Id { get; set; }

    public string CodigoDeLab { get; set; } = null!;

    public int? Capacidad { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<InventarioEquipo> InventarioEquipos { get; set; } = new List<InventarioEquipo>();

    public virtual ICollection<Iot> Iots { get; set; } = new List<Iot>();

    public virtual ICollection<PrestamosEspacio> PrestamosEspacios { get; set; } = new List<PrestamosEspacio>();
}
