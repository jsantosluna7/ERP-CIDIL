using ERP.Data.Modelos;
using Inventario.Abstraccion.Repositorio;
using Inventario.DTO.InventarioEquipoDTO;
using Microsoft.EntityFrameworkCore;

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
        public async Task<InventarioEquipo?> Actualizar(int id, ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO)
        {
            var invEquipoExiste = await GetById(id);
            if (invEquipoExiste == null)
            {
                return null;
            }
            
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
            invEquipoExiste.Disponible = actualizarInventarioEquipoDTO.Disponible;
            invEquipoExiste.IdEstadoFisico = actualizarInventarioEquipoDTO.IdEstadoFisico;
            invEquipoExiste.ValidacionPrestamo = actualizarInventarioEquipoDTO.ValidacionPrestamo;
            invEquipoExiste.Cantidad = actualizarInventarioEquipoDTO.Cantidad;
            invEquipoExiste.Activado = actualizarInventarioEquipoDTO.Activado;



            _context.Update(invEquipoExiste);
            _context.SaveChanges();
            var invEquipoActualizado = await GetById(id);
            return invEquipoActualizado;
        }

        //Se utiliza el metodo Crear para crear introducir los enquipos en el Base de Datos
        public async  Task<InventarioEquipo?> Crear(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
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
                Disponible = crearInventarioEquipoDTO.Disponible,
                IdEstadoFisico = crearInventarioEquipoDTO.IdEstadoFisico,
                ValidacionPrestamo = crearInventarioEquipoDTO.ValidacionPrestamo,
                Cantidad = crearInventarioEquipoDTO.Cantidad,
                Activado = crearInventarioEquipoDTO.Activado
            };

             _context.InventarioEquipos.Add(invEquipo);
            await _context.SaveChangesAsync();
            return invEquipo;


        }

        //Se utiliza el metodo Eliminar para Borrar los equipos deseados 
        public async Task<bool?> Eliminar(int id)
        {
            var inventarioEquipo = await GetById(id);
            if (inventarioEquipo == null)
            {
                return null;
            }
             _context.Remove(inventarioEquipo);
           await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> DesactivarEquipo(int id)
        {
            var equipo = await GetById(id);
            if (equipo == null)
            {
                return null;
            }
            equipo.Activado = false;
            _context.Update(equipo);
            await _context.SaveChangesAsync();
            return true;
        }

        // Metodo para llamar los equipos por ID
        public async Task<InventarioEquipo?> GetById(int id)
        {
            return await _context.InventarioEquipos.Include(i => i.SolicitudPrestamosDeEquipos).Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        //Metodo para optener todos los quipos reguistrados
        public async Task<List<InventarioEquipo>?> GetInventarioEquipos(int pagina, int tamanoPagina)
        {
            if(pagina <= 0) pagina = 1;
            if(tamanoPagina <= 0) tamanoPagina = 20;

            //var totalInventario = await _context.InventarioEquipos.CountAsync();
            //var totalPaginas = (int)Math.Ceiling(totalInventario / (double)tamanoPagina);

            return await _context.InventarioEquipos
                .Where(i => i.Activado == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
        }
    }
}
