using IoT.Abstraccion.Repositorio;
using IoT.Abstraccion.Servicios;
using IoT.Implementaciones.Repositorios;
using IoT.Implementaciones.Servicios;
using IoT.Modelos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Base de datos

builder.Services.AddDbContext<DbErpContext>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositorios
builder.Services.AddScoped<IRepositorioIoT, RepositorioIoT>();

//Servicios
builder.Services.AddScoped<IServicioIoT, ServicioIoT>();

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
