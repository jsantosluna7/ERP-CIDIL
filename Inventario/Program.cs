using Inventario.Abstraccion.Repositorio;
using Inventario.Abstraccion.Servicios;
using Inventario.Implementaciones.Repositorios;
using Inventario.Implementaciones.Servicios;
using Inventario.Modelos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//conectar la base de datos

builder.Services.AddDbContext<DbErpContext>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Añadir los Repositorios
builder.Services.AddScoped<IRepositorioInventarioEquipo, RepositorioInventarioEquipo>();
builder.Services.AddScoped<IRepositorioLaboratorio, RepositorioLaboratorio>();
builder.Services.AddScoped<IRepositorioEstadoFisico, RepositorioEstadoFisico>();

//Añadir los Servicios
builder.Services.AddScoped<IServicioInventarioEquipo, ServicioInventarioEquipo>();
builder.Services.AddScoped<IServicioLaboratorio, ServicioLaboratorio>();
builder.Services.AddScoped<IServicioEstadoFisico, ServicioEstadoFisico>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // o el puerto donde corre tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



//Variables de entorno
DotNetEnv.Env.Load();

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

app.UseCors("Angular");

app.Run();
