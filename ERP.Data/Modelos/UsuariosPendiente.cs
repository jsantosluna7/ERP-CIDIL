using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class UsuariosPendiente
{
    public Guid Id { get; set; }

    public int IdMatricula { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoUsuario { get; set; } = null!;

    public string CorreoInstitucional { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int? IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string OtpHash { get; set; } = null!;

    public DateTime OtpExpira { get; set; }

    public int? OtpIntentos { get; set; }
}
