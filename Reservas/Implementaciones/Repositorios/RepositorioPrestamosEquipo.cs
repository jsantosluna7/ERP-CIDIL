using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioPrestamosEquipo : IRepositorioPrestamosEquipo
    {


        private readonly DbErpContext _context;

        public RepositorioPrestamosEquipo(DbErpContext context)
        {
            _context = context;
        }




        public PrestamosEquipo Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
           var pEquipoExiste = GetById(id);

            pEquipoExiste.IdUsuario = actualizarPrestamosEquipoDTO.IdUsuario;
            pEquipoExiste.IdInventario = actualizarPrestamosEquipoDTO.IdInventario;
            pEquipoExiste.IdEstado = actualizarPrestamosEquipoDTO.IdEstado;
            pEquipoExiste.FechaInicio = actualizarPrestamosEquipoDTO.FechaInicio;
            pEquipoExiste.FechaFinal = actualizarPrestamosEquipoDTO.FechaFinal;
            pEquipoExiste.FechaEntrega = actualizarPrestamosEquipoDTO.FechaEntrega;
            pEquipoExiste.IdUsuarioAprobador = actualizarPrestamosEquipoDTO.IdUsuarioAprobador;
            pEquipoExiste.Motivo = actualizarPrestamosEquipoDTO.Motivo;
            pEquipoExiste.ComentarioAprobacion = actualizarPrestamosEquipoDTO.ComentarioAprobacion;


            _context.Update(pEquipoExiste);
            _context.SaveChanges();
            var pEquipoActualizado = GetById(id);
            return pEquipoActualizado;


        }

        public PrestamosEquipo Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var pEquipo = new PrestamosEquipo
            {
                IdUsuario = crearPrestamosEquipoDTO.IdUsuario,
                IdInventario = crearPrestamosEquipoDTO.IdInventario,
                IdEstado = crearPrestamosEquipoDTO.IdEstado,
                FechaInicio = crearPrestamosEquipoDTO.FechaInicio,
                FechaFinal = crearPrestamosEquipoDTO.FechaFinal,
                FechaEntrega = crearPrestamosEquipoDTO.FechaEntrega,
                IdUsuarioAprobador = crearPrestamosEquipoDTO.IdUsuarioAprobador,
                Motivo = crearPrestamosEquipoDTO.Motivo,
                ComentarioAprobacion = crearPrestamosEquipoDTO.ComentarioAprobacion

            };

            _context.PrestamosEquipos.Add(pEquipo);
            _context.SaveChanges();
            return pEquipo;
        }

        public void Eliminar(int id)
        {
           PrestamosEquipo prestamosEquipo = GetById(id);
            _context.Remove(prestamosEquipo);
            _context.SaveChanges();
        }

        public PrestamosEquipo GetById(int id)
        {
            return _context.PrestamosEquipos.Where(p => p.Id == id).FirstOrDefault();
        }

        public List<PrestamosEquipo> GetPrestamosEquipo()
        {
           return [.._context.PrestamosEquipos];
        }
    }
}
