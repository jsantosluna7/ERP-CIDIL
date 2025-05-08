using Inventario.Abstraccion.Repositorio;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Modelos;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace Inventario.Implementaciones.Repositorios
{
    public class RepositorioInventarioEquipo : IRepositorioInventarioEquipo
    {
        //Hacemos una inyeccion de dependencia 
        private readonly DbErpContext _context;

        public RepositorioInventarioEquipo(DbErpContext context)
        {
            _context = context;
        }

        //Se utiliza el metodo Actualizar para actualizar los equipos del inventario
        public InventarioEquipo Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO)
        {
            var invEquipoExiste = GetById(id);

            invEquipoExiste.Nombre = actualizarInventarioEquipoDTO.Nombre;
            invEquipoExiste.NombreCorto = actualizarInventarioEquipoDTO.NombreCorto;
            invEquipoExiste.Perfil = actualizarInventarioEquipoDTO.Perfil;
            invEquipoExiste.IdLaboratorio = actualizarInventarioEquipoDTO.IdLaboratorio;
            invEquipoExiste.Fabricante = actualizarInventarioEquipoDTO.Fabricante;
            invEquipoExiste.Modelo = actualizarInventarioEquipoDTO.Modelo;
            invEquipoExiste.Serial = actualizarInventarioEquipoDTO.Serial;
            invEquipoExiste.DescripcionLarga = actualizarInventarioEquipoDTO.DescripcionLarga;
            invEquipoExiste.FechaTransaccion = actualizarInventarioEquipoDTO.FechaTransaccion;
            invEquipoExiste.Departamento = actualizarInventarioEquipoDTO.Departamento;
            invEquipoExiste.ImporteActivo = actualizarInventarioEquipoDTO.ImporteActivo;
            invEquipoExiste.ImagenEquipo = actualizarInventarioEquipoDTO.ImagenEquipo;
            invEquipoExiste.Estado = actualizarInventarioEquipoDTO.Estado;



            _context.Update(invEquipoExiste);
            _context.SaveChanges();
            var invEquipoActualizado = GetById(id);
            return invEquipoActualizado;
        }

        //Se utiliza el metodo Crear para crear introducir los enquipos en el Base de Datos
        public InventarioEquipo Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
        {

            var invEquipo = new InventarioEquipo
            {
                Nombre = crearInventarioEquipoDTO.Nombre,
                NombreCorto = crearInventarioEquipoDTO.NombreCorto,
                Perfil = crearInventarioEquipoDTO.Perfil,
                IdLaboratorio = crearInventarioEquipoDTO.IdLaboratorio,
                Fabricante = crearInventarioEquipoDTO.Fabricante,
                Modelo = crearInventarioEquipoDTO.Modelo,
                Serial = crearInventarioEquipoDTO.Serial,
                DescripcionLarga = crearInventarioEquipoDTO.DescripcionLarga,
                FechaTransaccion = crearInventarioEquipoDTO.FechaTransaccion,
                Departamento = crearInventarioEquipoDTO.Departamento,
                ImporteActivo = crearInventarioEquipoDTO.ImporteActivo,
                ImagenEquipo = crearInventarioEquipoDTO.ImagenEquipo,
                Estado = crearInventarioEquipoDTO.Estado
            };

            _context.InventarioEquipos.Add(invEquipo);
            _context.SaveChanges();
            return invEquipo;


        }

        //Se utiliza el metodo Eliminar para Borrar los equipos deseados 
        public void Eliminar(int id)
        {
            InventarioEquipo inventarioEquipo = GetById(id);
            _context.Remove(inventarioEquipo);
            _context.SaveChanges();
        }

        // Metodo para llamar los equipos por ID
        public InventarioEquipo GetById(int id)
        {
            return _context.InventarioEquipos.Where(i => i.Id == id).FirstOrDefault();
        }

        //Metodo para optener todos los quipos reguistrados
        public List<InventarioEquipo> GetInventarioEquipos()
        {
           return [.._context.InventarioEquipos];
        }
    }
}
