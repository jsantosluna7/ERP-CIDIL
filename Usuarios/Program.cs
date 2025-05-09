using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.Controllers;
using Usuarios.Implementaciones.Repositorios;
using Usuarios.Implementaciones.Servicios;
using Usuarios.Modelos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Conectar la base de datos
builder.Services.AddDbContext<DbErpContext>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetion")));

//Añadimos los repositorios
builder.Services.AddScoped<IRepositorioRoles, RepositorioRoles>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

//Añadimos el servicio
builder.Services.AddScoped<IServicioRoles, ServicioRoles>();
builder.Services.AddScoped<IServicioUsuarios, ServicioUsuarios>();

////Añadiendo al ensamblado principal
//builder.Services.AddControllers()
//    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(RolController).Assembly));

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
