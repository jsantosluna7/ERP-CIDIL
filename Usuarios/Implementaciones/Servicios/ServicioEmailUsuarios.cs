using System.Net.Mail;
using Microsoft.Extensions.Options;
using Usuarios.Modelos;
using static System.Net.WebRequestMethods;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioEmailUsuarios
    {
        private readonly SmtpSettings _configuracion;

        public ServicioEmailUsuarios(IOptions<SmtpSettings> configuracion)
        {
            _configuracion = configuracion.Value;
        }

        public async Task EnviarCorreoRecuperacion(string destinatario, string token)
        {
            var link = $"https://cidilipl.online/cambiar-contrasena?token={token}";
            // Cargar la plantilla HTML desde un archivo
            string htmlTemplate = System.IO.File.ReadAllText("Templates/cambiar_contrasena_plantilla.html");
            // Reemplazar el marcador de posición en la plantilla con el OTP
            string html = htmlTemplate.Replace("{{RESET_LINK}}", link);

            //var html = $"<p>Hola,</p>\r\n<p>Has solicitado restablecer tu contraseña.</p>\r\n<p><a href=\"{link}\">Haz clic aquí para continuar</a></p>\r\n<p>Si no fuiste tú, puedes ignorar este mensaje.</p>\r\n";

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

        public async Task EnviarCorreoOtp(string destinatario, string otp)
        {
            // Cargar la plantilla HTML desde un archivo
            string htmlTemplate = System.IO.File.ReadAllText("Templates/otp_plantilla.html");
            // Reemplazar el marcador de posición en la plantilla con el OTP
            string html = htmlTemplate.Replace("{{OTP_CODE}}", otp);

            //var html = $"<p>Hola,</p>\r\n<p>Has solicitado registrarte, aquí está el codigo OTP.</p>\r\n<p>{otp} introducelo en la página para poder continuar.</a></p>\r\n<p>Si no fuiste tú, puedes ignorar este mensaje.</p>\r\n";

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Código OTP";
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

            // Cargar la plantilla HTML desde un archivo
            string htmlTemplate = System.IO.File.ReadAllText("Templates/contrasena_cambiada_plantilla.html");

            mensaje.To.Add(destinatario);
            mensaje.Subject = "Cambio de contraseña exitoso";
            mensaje.Body = htmlTemplate;
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
