using System;
using System.Collections.Generic;

namespace Reservas.Modelos;

public partial class InventarioEquipo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? NombreCorto { get; set; }

    public string? Perfil { get; set; }

    public int IdLaboratorio { get; set; }

    public string? Fabricante { get; set; }

    public string? Modelo { get; set; }

    public string Serial { get; set; } = null!;

    public string? DescripcionLarga { get; set; }

    public DateTime? FechaTransaccion { get; set; }

    public string? Departamento { get; set; }

    public decimal? ImporteActivo { get; set; }

    public byte[]? ImagenEquipo { get; set; }

    public bool? Estado { get; set; }

    public virtual Laboratorio IdLaboratorioNavigation { get; set; } = null!;

    public virtual ICollection<PrestamosEquipo> PrestamosEquipos { get; set; } = new List<PrestamosEquipo>();
}
