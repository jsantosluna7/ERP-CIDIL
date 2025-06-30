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
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.Implementaciones.Repositorios;
using Reservas.Implementaciones.Servicios;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.Implementaciones.Repositorios;
using Usuarios.Implementaciones.Servicios;
using Usuarios.Modelos;

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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var app = builder.Build();

app.UseStaticFiles(); // Ya sirve wwwroot automáticamente

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
