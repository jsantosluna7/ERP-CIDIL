using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.Implementaciones.Repositorios;
using Reservas.Implementaciones.Servicios;
using Reservas.Modelos;

var builder = WebApplication.CreateBuilder(args);

//Conectar la base de datos 

builder.Services.AddDbContext<DbErpContext>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Añadir Repositorios

builder.Services.AddScoped<IRepositorioPrestamosEquipo, RepositorioPrestamosEquipo>();
builder.Services.AddScoped<IRepositorioEstado, RepositorioEstado>();
builder.Services.AddScoped<IRepositorioHorario, RepositorioHorario>();
builder.Services.AddScoped<IRepositorioReservaDeEspacio, RepositorioReservaDeEspacio>();
builder.Services.AddScoped<IRepositorioSolicitudDeReserva, RepositorioSolicitudDeReserva>();

//Añadir Servicios

builder.Services.AddScoped<IServicioPrestamosEquipo, ServicioPrestamosEquipo>();
builder.Services.AddScoped<IServicioEstado, ServicioEstado>();
builder.Services.AddScoped<IServicioHorario, ServicioHorario>();
builder.Services.AddScoped<IServicioReservaDeEspacio, ServicioReservaDeEspacio>();
builder.Services.AddScoped<IServicioSolicitudDeReserva, ServicioSolicitudDeReserva>();

//Añadimos el servicio de email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<ServicioEmail>();
builder.Services.AddScoped<ServicioConflictos>();


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("PermitirFrontend", policy =>
//    {
//        policy.WithOrigins("https://localhost:7253/swagger/index.html") // o el puerto donde corre tu frontend
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});


var app = builder.Build();

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
