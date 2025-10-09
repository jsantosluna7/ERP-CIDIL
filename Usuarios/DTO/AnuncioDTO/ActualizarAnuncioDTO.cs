namespace Usuarios.DTO.AnuncioDTO
{
    /// <summary>
    /// DTO para actualizar un anuncio existente.
    /// Incluye título, descripción y URL de la imagen.
    /// </summary>
    public class ActualizarAnuncioDTO
    {
        /// <summary>
        /// Nuevo título del anuncio (opcional).
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Nueva descripción del anuncio (opcional).
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Nueva URL de la imagen del anuncio (opcional).
        /// </summary>
        public string? ImagenUrl { get; set; }
    }
}
