namespace Usuarios.DTO.LoginDTO
{
    public class VerificarOtpDTO
    {
        public Guid PendingUserId { get; set; }
        public string Otp { get; set; }
    }
}
