using System.Net.Mail;
using Microsoft.Extensions.Options;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioEmail
    {
        private readonly SmtpSettings _configuracion;

        public ServicioEmail(IOptions<SmtpSettings> configuracion)
        {
            _configuracion = configuracion.Value;
        }

        public async Task EnviarCorreoRecuperacion(string destinatario, string token)
        {
            var link = $"https://localhost:5001/ResetPassword?token={token}";

            var html = $"<p>Hola,</p>\r\n<p>Has solicitado restablecer tu contraseña.</p>\r\n<p><a href=\"{link}\">Haz clic aquí para continuar</a></p>\r\n<p>Si no fuiste tú, puedes ignorar este mensaje.</p>\r\n";

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Restablecimiento de contraseña";
            mensaje.IsBodyHtml = true;
            mensaje.Body = html;
            mensaje.ReplyToList.Add(new MailAddress(_configuracion.User));
            mensaje.Priority = MailPriority.High;
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
            mensaje.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            // Agregar encabezados adicionales
            mensaje.Headers.Add("X-Priority", "1");
            mensaje.Headers.Add("X-MSMail-Priority", "High");
            mensaje.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
            mensaje.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
            mensaje.Headers.Add("X-Message-Flag", "FollowUp");
            mensaje.Headers.Add("X-OriginalArrivalTime", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss") + " GMT");
            mensaje.Headers.Add("X-OriginalMessage-ID", Guid.NewGuid().ToString());
            mensaje.Headers.Add("X-OriginalSender", _configuracion.User);
            mensaje.Headers.Add("X-OriginalRecipient", destinatario);

            using var smtp = new SmtpClient(_configuracion.Host, _configuracion.Port)
            {
                EnableSsl = _configuracion.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_configuracion.User, _configuracion.Password)
            };

            await smtp.SendMailAsync(mensaje);

        }

        public async Task CorreoCambioExitoso(string destinatario)
        {
            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Cambio de contraseña exitoso";
            mensaje.Body = $"<h1>Cambio de contraseña</h1><p>Querido {destinatario}, </p><p>Su contraseña ha sido modificada con exito.</p>";
            mensaje.IsBodyHtml = true;
            mensaje.Priority = MailPriority.High;
            mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
            mensaje.BodyEncoding = System.Text.Encoding.UTF8;
            mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
            mensaje.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            // Agregar encabezados adicionales
            mensaje.Headers.Add("X-Priority", "1");
            mensaje.Headers.Add("X-MSMail-Priority", "High");
            mensaje.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
            mensaje.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
            mensaje.Headers.Add("X-Message-Flag", "FollowUp");
            mensaje.Headers.Add("X-OriginalArrivalTime", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss") + " GMT");
            mensaje.Headers.Add("X-OriginalMessage-ID", Guid.NewGuid().ToString());
            mensaje.Headers.Add("X-OriginalSender", _configuracion.User);
            mensaje.Headers.Add("X-OriginalRecipient", destinatario);

            using var smtp = new SmtpClient(_configuracion.Host, _configuracion.Port)
            {
                EnableSsl = _configuracion.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_configuracion.User, _configuracion.Password)
            };

            await smtp.SendMailAsync(mensaje);

        }
    }
}
