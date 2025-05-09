using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioPrestamosEquipo : IServicioPrestamosEquipo
    {


        private readonly IRepositorioPrestamosEquipo repositorioPrestamosEquipo;


        public ServicioPrestamosEquipo(IRepositorioPrestamosEquipo rPrestamosEquipo)
        {
            this.repositorioPrestamosEquipo = rPrestamosEquipo;
        }




        public PrestamosEquipoDTO Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
           var pEquipo = repositorioPrestamosEquipo.Actualizar(id,actualizarPrestamosEquipoDTO);
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

        public PrestamosEquipoDTO Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var pEquipo = repositorioPrestamosEquipo.Crear(crearPrestamosEquipoDTO);
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

        public void Eliminar(int id)
        {
            repositorioPrestamosEquipo.Eliminar(id);
        }

        public PrestamosEquipo GetById(int id)
        {
            return repositorioPrestamosEquipo.GetById(id);
        }

        public List<PrestamosEquipoDTO> GetPrestamosEquipo()
        {
            var pEquipo = repositorioPrestamosEquipo.GetPrestamosEquipo();
            var pEquipoDTO = new List<PrestamosEquipoDTO>();
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
    }
}
