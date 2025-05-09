using System;
using System.Collections.Generic;

namespace Usuarios.Modelos;

public partial class Roles
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
