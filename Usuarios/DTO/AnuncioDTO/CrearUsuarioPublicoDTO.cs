using System.ComponentModel.DataAnnotations;

public class CrearUsuarioPublicoDTO
{
    [Required]
    public string Nombre { get; set; }

    [Required, EmailAddress]
    public string Correo { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
