namespace Usuarios.Modelos
{
    public class ResetearContrasena
    {
        public required string Token { get; set; }
        public required string NuevaContrasena { get; set; }
    }
}
