using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ERP.Data.Modelos;

public partial class ComentariosOrden
{
    public int Id { get; set; }

    public int? OrdenId { get; set; }

    public int? ItemId { get; set; }

    public int? UsuarioId { get; set; }

    public string Comentario { get; set; } = null!;

    public DateTime? CreadoEn { get; set; }
    [JsonIgnore]
    public virtual OrdenItem? Item { get; set; }
    [JsonIgnore]
    public virtual Ordene? Orden { get; set; }
    [JsonIgnore]
    public virtual Usuario? Usuario { get; set; }
}
