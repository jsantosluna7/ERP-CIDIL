using System;
using System.Collections.Generic;

namespace ERP.Data.Modelos;

public partial class Comentario
{
    public int Id { get; set; }

    public int? OrdenId { get; set; }

    public int? ItemId { get; set; }

    public int? UsuarioId { get; set; }

    public string Comentario1 { get; set; } = null!;

    public DateTime? CreadoEn { get; set; }

    public virtual OrdenItem? Item { get; set; }

    public virtual Ordene? Orden { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
