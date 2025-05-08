using System;
using System.Collections.Generic;

namespace Inventario.Modelos;

public partial class Usuario
{
    public int Id { get; set; }

    public int IdMatricula { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string CorreoInstitucional { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaUltimaModificacion { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<PrestamosEquipo> PrestamosEquipoIdUsuarioAprobadorNavigations { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<PrestamosEquipo> PrestamosEquipoIdUsuarioNavigations { get; set; } = new List<PrestamosEquipo>();

    public virtual ICollection<PrestamosEspacio> PrestamosEspacioIdUsuarioAprobadorNavigations { get; set; } = new List<PrestamosEspacio>();

    public virtual ICollection<PrestamosEspacio> PrestamosEspacioIdUsuarioNavigations { get; set; } = new List<PrestamosEspacio>();
}
