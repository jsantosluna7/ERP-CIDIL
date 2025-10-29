namespace Usuarios.DTO.Comentarios
{
    public class CrearComentarioDTO
    {
        public int AnuncioId { get; set; }

        // ID del usuario registrado que comenta
        public int UsuarioId { get; set; }

        // Se añade la propiedad NombreUsuario para que el backend 
        // pueda recibir y guardar el dato que envía la aplicación de Angular.
        public string? NombreUsuario { get; set; }

        public string Texto { get; set; } = string.Empty;
    }
}
