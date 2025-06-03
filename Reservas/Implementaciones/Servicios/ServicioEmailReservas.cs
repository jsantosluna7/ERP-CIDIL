using System.Net.Mail;
using Microsoft.Extensions.Options;
using Usuarios.Modelos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioEmailReservas
    {
        private readonly SmtpSettings _configuracion;

        public ServicioEmailReservas(IOptions<SmtpSettings> configuracion)
        {
            _configuracion = configuracion.Value;
        }

        public async Task EnviarCorreoReserva(string destinatario)
        {
            var html = $"<p>Hola,</p>\r\n<p>Te ha llegado una solitud para reservar un espacio.</p>\r\n"; //agregar url para redirigir al front.

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Solicitud para reservar un espacio.";
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

        public async Task EnviarCorreoReservaMoficación(string destinatario, string nombreUsuario, string apellidoUsuario)
        {

            var html = $"<p>Hola,</p>\r\n<p>El usuario llamado <strong>{nombreUsuario} {apellidoUsuario}</strong> ha modificado su reserva.</p>\r\n";

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = $"Actualización de Solicitud de Reserva, {nombreUsuario} {apellidoUsuario}";
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
    }
}
