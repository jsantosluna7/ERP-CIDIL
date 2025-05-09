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

//A�adir Repositorios

builder.Services.AddScoped<IRepositorioPrestamosEquipo, RepositorioPrestamosEquipo>();
builder.Services.AddScoped<IRepositorioEstado, RepositorioEstado>();

//A�adir Servicios

builder.Services.AddScoped<IServicioPrestamosEquipo, ServicioPrestamosEquipo>();
builder.Services.AddScoped<IServicioEstado, ServicioEstado>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7253/swagger/index.html") // o el puerto donde corre tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


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
