using System.Net.Mail;
using ERP.Data.Modelos;
using Microsoft.Extensions.Options;
using Usuarios.Modelos;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

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
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "solicitud_reserva_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            var link = $"https://cidilipl.online/home/solicitud-laboratorio";

            // Reemplazar el marcador de posición en la plantilla con el OTP
            string html = htmlTemplate.Replace("{{FRONTEND_RESERVA_URL}}", link);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Solicitud para reservar un laboratorio.";
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
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "modificacion_reserva_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar el marcador de posición en la plantilla con el OTP
            var html = htmlTemplate.Replace("{{NOMBRE_USUARIO}}", nombreUsuario).Replace("{{APELLIDO_USUARIO}}", apellidoUsuario);

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

        public async Task EnviarCorreoAprobacion(string destinatario, string nombreUsuario, string apellidoUsuario, string espacio, string fechaSolicitud, string fechaReserva, string horaInicio, string horaFin)
        {
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "aprobacion_reserva_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar el marcador de posición en la plantilla con el OTP
            var html = htmlTemplate.Replace("{{NOMBRE_USUARIO}}", nombreUsuario).Replace("{{APELLIDO_USUARIO}}", apellidoUsuario).Replace("{{NOMBRE_ESPACIO}}", espacio).Replace("{{FECHA_SOLICITUD}}", fechaSolicitud).Replace("{{FECHA_RESERVA}}", fechaReserva).Replace("{{HORA_INICIO}}", horaInicio).Replace("{{HORA_FIN}}", horaFin);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = $"Aprobación de Solicitud de reserva de espacio, {espacio}";
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

        public async Task EnviarCorreoRechazo(string destinatario, string nombreUsuario, string apellidoUsuario, string espacio, string fechaSolicitud, string fechaReserva, string motivo_rechazo, string horaInicio, string horaFin)
        {
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "rechazo_reserva_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar el marcador de posición en la plantilla con el OTP
            var html = htmlTemplate.Replace("{{NOMBRE_USUARIO}}", nombreUsuario).Replace("{{APELLIDO_USUARIO}}", apellidoUsuario).Replace("{{NOMBRE_ESPACIO}}", espacio).Replace("{{FECHA_SOLICITUD}}", fechaSolicitud).Replace("{{FECHA_RESERVA}}", fechaReserva).Replace("{{MOTIVO_RECHAZO}}", motivo_rechazo).Replace("{{HORA_INICIO}}", horaInicio).Replace("{{HORA_FIN}}", horaFin);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = $"Rechazo de Solicitud de reserva de espacio, {espacio}";
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

        public async Task EnviarCorreoReservaEquipos(string destinatario)
        {
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "solicitud_reserva_equipo_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            var link = $"https://cidilipl.online/home/solicitud-equipo";

            // Reemplazar el marcador de posición en la plantilla con el OTP
            string html = htmlTemplate.Replace("{{FRONTEND_RESERVA_URL}}", link);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = "Solicitud de Reserva de Equipo";
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

        public async Task EnviarCorreoAprobacionEquipos(string destinatario, string nombreUsuario, string apellidoUsuario, string fechaReserva, string fechaEntrega, string fechaFin)
        {
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "aprobacion_reserva_equipo_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar el marcador de posición en la plantilla con el OTP
            var html = htmlTemplate.Replace("{{NOMBRE_USUARIO}}", nombreUsuario).Replace("{{APELLIDO_USUARIO}}", apellidoUsuario).Replace("{{FECHA_RESERVA}}", fechaReserva).Replace("{{HORA_INICIO}}", fechaEntrega).Replace("{{HORA_FIN}}", fechaFin);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = $"Aprobación de Solicitud de reserva de equipo";
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

        public async Task EnviarCorreoRechazoEquipos(string destinatario, string nombreUsuario, string apellidoUsuario, string fechaReserva, string motivo_rechazo, string fechaEntrega, string fechaFin)
        {
            // Cargar la plantilla HTML desde un archivo
            var basePath = AppContext.BaseDirectory;
            var templatePath = Path.Combine(basePath, "Templates", "rechazo_reserva_equipo_plantilla.html");
            string htmlTemplate = File.ReadAllText(templatePath);

            // Reemplazar el marcador de posición en la plantilla con el OTP
            var html = htmlTemplate.Replace("{{NOMBRE_USUARIO}}", nombreUsuario).Replace("{{APELLIDO_USUARIO}}", apellidoUsuario).Replace("{{FECHA_RESERVA}}", fechaReserva).Replace("{{MOTIVO_RECHAZO}}", motivo_rechazo).Replace("{{HORA_INICIO}}", fechaEntrega).Replace("{{HORA_FIN}}", fechaFin);

            // Configuración del cliente SMTP
            var mensaje = new MailMessage
            {
                From = new MailAddress(_configuracion.User, "ERP CIDIL")
            };
            mensaje.To.Add(destinatario);
            mensaje.Subject = $"Rechazo de Solicitud de reserva de equipo";
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
