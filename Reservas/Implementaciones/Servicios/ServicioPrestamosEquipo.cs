using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;
using System.Xml;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioPrestamosEquipo : IServicioPrestamosEquipo
    {


        private readonly IRepositorioPrestamosEquipo repositorioPrestamosEquipo;


        public ServicioPrestamosEquipo(IRepositorioPrestamosEquipo rPrestamosEquipo)
        {
            this.repositorioPrestamosEquipo = rPrestamosEquipo;
        }


        public async Task<PrestamosEquipoDTO?> Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
           var pEquipo = await repositorioPrestamosEquipo.Actualizar(id,actualizarPrestamosEquipoDTO);
            if (pEquipo == null)
            {
                return null;
            }
            var pEquipoDTO = new PrestamosEquipoDTO
            {
                IdUsuario = pEquipo.IdUsuario,
                IdInventario = pEquipo.IdInventario,
                IdEstado = pEquipo.IdEstado,
                FechaInicio = pEquipo.FechaInicio,
                FechaFinal = pEquipo.FechaFinal,
                FechaEntrega =  pEquipo.FechaEntrega,
                IdUsuarioAprobador =pEquipo.IdUsuarioAprobador,
                Motivo = pEquipo.Motivo,
                ComentarioAprobacion = pEquipo.ComentarioAprobacion,
            };

            return pEquipoDTO;
        }

        public async Task<PrestamosEquipoDTO?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var pEquipo = await repositorioPrestamosEquipo.Crear(crearPrestamosEquipoDTO);
            if (pEquipo == null)
            {
                return null;
            }
            var pEquipoDTO = new PrestamosEquipoDTO
            {
                IdUsuario = pEquipo.IdUsuario,
                IdInventario = pEquipo.IdInventario,
                IdEstado = pEquipo.IdEstado,
                FechaInicio = pEquipo.FechaInicio,
                FechaFinal = pEquipo.FechaFinal,
                FechaEntrega = pEquipo.FechaEntrega,
                IdUsuarioAprobador = pEquipo.IdUsuarioAprobador,
                Motivo = pEquipo.Motivo,
                ComentarioAprobacion = pEquipo.ComentarioAprobacion,
            };
            return pEquipoDTO;
        }

        public async Task<bool?> Eliminar(int id)
        {
          var e = await repositorioPrestamosEquipo.Eliminar(id);
            if (e == null)
            {
                return null;
            }
            return e;
        }

        public async Task<PrestamosEquipo?> GetById(int id)
        {
            return await repositorioPrestamosEquipo.GetById(id);
        }

        public async Task<List<PrestamosEquipoDTO>?> GetPrestamosEquipo()
        {
            var pEquipo = await repositorioPrestamosEquipo.GetPrestamosEquipo();
            if (pEquipo == null)
            {
                return null;
            }
            var pEquipoDTO = new List<PrestamosEquipoDTO>();
            //foreach (var p in pEquipo)
            //{
            //    return pEquipoDTO;
            //}
            foreach(PrestamosEquipo prestamosEquipo in pEquipo)
            {
                var nuevoPEquipo = new PrestamosEquipoDTO
                {   
                    Id = prestamosEquipo.Id,
                    IdUsuario = prestamosEquipo.IdUsuario,
                    IdInventario = prestamosEquipo.IdInventario,
                    IdEstado = prestamosEquipo.IdEstado,
                    FechaInicio = prestamosEquipo.FechaInicio,
                    FechaFinal = prestamosEquipo.FechaFinal,
                    FechaEntrega = prestamosEquipo.FechaEntrega,
                    IdUsuarioAprobador = prestamosEquipo.IdUsuarioAprobador,
                    Motivo = prestamosEquipo.Motivo,
                    ComentarioAprobacion = prestamosEquipo.ComentarioAprobacion,
                };
                pEquipoDTO.Add(nuevoPEquipo);
            }
            return pEquipoDTO;
        }

        // Método para desactivar un prestamo
        public async Task<bool?> DesactivarPrestamoEquipos(int id)
        {
            // Obtener el prestamo del repositorio
            var equipo = await repositorioPrestamosEquipo.GetById(id);
            if (equipo == null)
            {
                return null;
            }
            // Desactivar el prestamo
            equipo.Activado = false;
            await repositorioPrestamosEquipo.desactivarPrestamoEquipos(id);
            return true;
        }
    }
}
