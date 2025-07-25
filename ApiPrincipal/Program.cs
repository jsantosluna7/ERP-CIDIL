using System.Net.Mail;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.Implementaciones.Repositorios;
using Inventario.Implementaciones.Servicios;
using IoT.Abstraccion.Repositorio;
using IoT.Abstraccion.Servicios;
using IoT.Implementaciones.Repositorios;
using IoT.Implementaciones.Servicios;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.Implementaciones.Repositorios;
using Reservas.Implementaciones.Servicios;
using StackExchange.Redis;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.Implementaciones.Repositorios;
using Usuarios.Implementaciones.Servicios;
using Usuarios.Modelos;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// A�adir los Repositorios
builder.Services.AddScoped<IRepositorioInventarioEquipo, RepositorioInventarioEquipo>();
builder.Services.AddScoped<IRepositorioLaboratorio, RepositorioLaboratorio>();
builder.Services.AddScoped<IRepositorioEstadoFisico, RepositorioEstadoFisico>();
builder.Services.AddScoped<IRepositorioIoT, RepositorioIoT>();
builder.Services.AddScoped<IRepositorioPrestamosEquipo, RepositorioPrestamosEquipo>();
builder.Services.AddScoped<IRepositorioEstado, RepositorioEstado>();
builder.Services.AddScoped<IRepositorioHorario, RepositorioHorario>();
builder.Services.AddScoped<IRepositorioReservaDeEspacio, RepositorioReservaDeEspacio>();
builder.Services.AddScoped<IRepositorioSolicitudDeReserva, RepositorioSolicitudDeReserva>();
builder.Services.AddScoped<IRepositorioSolicitudPrestamosDeEquipos, RepositorioSolicitudPrestamosDeEquipos>();
builder.Services.AddScoped<IRepositorioRoles, RepositorioRoles>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioLogin, RepositorioLogin>();
builder.Services.AddScoped<IRepositorioResetPassword, RepositorioResetPassword>();
builder.Services.AddScoped<IRepositorioReporteFalla, RepositorioReporteFalla>();



//A�adir los Servicios
builder.Services.AddScoped<IServicioInventarioEquipo, ServicioInventarioEquipo>();
builder.Services.AddScoped<IServicioLaboratorio, ServicioLaboratorio>();
builder.Services.AddScoped<IServicioEstadoFisico, ServicioEstadoFisico>();
builder.Services.AddScoped<IServicioIoT, ServicioIoT>();
builder.Services.AddScoped<IServicioPrestamosEquipo, ServicioPrestamosEquipo>();
builder.Services.AddScoped<IServicioEstado, ServicioEstado>();
builder.Services.AddScoped<IServicioHorario, ServicioHorario>();
builder.Services.AddScoped<IServicioReservaDeEspacio, ServicioReservaDeEspacio>();
builder.Services.AddScoped<IServicioSolicitudDeReserva, ServicioSolicitudDeReserva>();
builder.Services.AddScoped<IServicioSolicitudPrestamosDeEquipos, ServicioSolicitudPrestamosDeEquipos>();
builder.Services.AddScoped<IServicioRoles, ServicioRoles>();
builder.Services.AddScoped<IServicioUsuarios, ServicioUsuarios>();
builder.Services.AddScoped<IServicioLogin, ServicioLogin>();
builder.Services.AddScoped<IServicioResetPassword, ServicioResetPassword>();
builder.Services.AddScoped<IServicioReporteFalla, ServicioReporteFalla>();

//A�adimos el servicio de email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<ServicioEmailUsuarios>();
builder.Services.AddScoped<ServicioEmailReservas>();

//A�adimos el servicio de email
//builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<ServicioConflictos>();

// Cargar variables de entorno desde el archivo .env
DotNetEnv.Env.Load();

// Construir la cadena de conexi�n
var connectionString = $"Host={Environment.GetEnvironmentVariable("HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("USERNAME")};" +
                       $"Password={Environment.GetEnvironmentVariable("PASSWORD")}";


// Registrar el DbContext

builder.Services.AddDbContext<DbErpContext>(options =>
    options.UseNpgsql(connectionString));

// Telegram config
var config = builder.Configuration.GetSection("Telegram");
var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")!;
var telegramChatId = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID")!;
var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST")!;
var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")!;
var smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!);
var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER")!;
var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS")!;
var alertEmailTo = Environment.GetEnvironmentVariable("ALERT_EMAIL_TO")!;

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear(); // Trust all networks
    options.KnownProxies.Clear();  // Trust all proxies
});


// Redis config
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisHost)); //Servidor de Redis

// Agregar política CORS global que permite todo
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirSoloMiApp", policy =>
    {
        policy
            .WithOrigins("https://cidilipl.online",
                         "http://localhost:4200") // Reemplaza con el dominio real de tu frontend
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Rate Limiter (100 req/min por IP)
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("LimiteGlobal", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: GetClientIp(context),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddHttpClient();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

async Task SendAlertEmail(string ip, string path, DateTime now)
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Tu API", smtpUser));
    message.To.Add(MailboxAddress.Parse(alertEmailTo));
    message.Subject = $"[API] IP bloqueada: {ip}";

    var body = new BodyBuilder
    {
        HtmlBody = $"<p>La IP <strong>{ip}</strong> fue bloqueada en <em>{path}</em> a las {now:yyyy‑MM‑dd HH:mm:ss} UTC.</p>"
    };
    message.Body = body.ToMessageBody();

    using var smtp = new MailKit.Net.Smtp.SmtpClient();
    await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync(smtpUser, smtpPass);
    await smtp.SendAsync(message);
    await smtp.DisconnectAsync(true);
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ya sirve wwwroot automáticamente
app.UseForwardedHeaders(); // Para manejar encabezados X-Forwarded-For y X-Forwarded-Proto
app.UseCors("PermitirSoloMiApp");
// Middleware de bloqueo por IP
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var redis = context.RequestServices.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
    var httpClient = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();

    var forwardedIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var ip = !string.IsNullOrWhiteSpace(forwardedIp)
        ? forwardedIp.Split(',')[0].Trim()
        : context.Connection.RemoteIpAddress?.ToString();
    var path = context.Request.Path;

    if (ip == null)
    {
        await next();
        return;
    }

    var bloqueadaKey = $"ip:bloqueada:{ip}";
    var contadorKey = $"ip:contador:{ip}";

    if (await redis.KeyExistsAsync(bloqueadaKey))
    {
        logger.LogWarning("Intento de acceso bloqueado: {IP} - {Path}", ip, path);
        context.Response.StatusCode = 429;
        await context.Response.WriteAsync("IP bloqueada por exceso de peticiones. Intenta en 24 horas.");
        return;
    }

    var count = await redis.StringIncrementAsync(contadorKey);
    if (count == 1)
    {
        await redis.KeyExpireAsync(contadorKey, TimeSpan.FromMinutes(1));
    }

    if (count > 100)
    {
        await redis.StringSetAsync(bloqueadaKey, "1", TimeSpan.FromHours(24));
        var now = DateTime.UtcNow;

        logger.LogError("IP bloqueada por abuso: {IP} - {Path} - {Time}", ip, path, now);

        // Enviar alerta a Telegram
        var mensaje = $"🚨 *ALERTA DE ABUSO*\nIP: `{ip}`\nRuta: `{path}`\nHora: `{now:yyyy-MM-dd HH:mm:ss}` UTC";
        var urlTelegram = $"https://api.telegram.org/bot{telegramBotToken}/sendMessage";
        var parametros = new Dictionary<string, string>
        {
            { "chat_id", telegramChatId },
            { "text", mensaje },
            { "parse_mode", "Markdown" }
        };

        try
        {
            await httpClient.PostAsync(urlTelegram, new FormUrlEncodedContent(parametros));
            await SendAlertEmail(ip, path, now);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al enviar alerta Telegram: {Error}", ex.Message);
        }

        context.Response.StatusCode = 429;
        await context.Response.WriteAsync("IP bloqueada por exceder el límite. Intenta en 24 horas.");
        return;
    }

    await next();
});
app.Use(async (context, next) =>
{
    var ip = context.Connection.RemoteIpAddress?.ToString();
    var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("RemoteIp: {RemoteIp}, Forwarded: {Forwarded}", ip, forwarded);

    await next();
});
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("LimiteGlobal"); // Aplicar a todos los endpoints

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

// Función utilitaria para obtener la IP real
static string GetClientIp(HttpContext context)
{
    return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim()
       ?? context.Connection.RemoteIpAddress?.ToString()
       ?? "unknown";
}

