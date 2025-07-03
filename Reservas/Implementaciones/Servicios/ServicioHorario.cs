using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Reservas.Abstraccion.Repositorio;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOHorario;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioHorario : IServicioHorario
    {
        private readonly IRepositorioHorario _repositorioHorario;
        public ServicioHorario(IRepositorioHorario repositorioHorario)
        {
            _repositorioHorario = repositorioHorario;
        }

        // Método para obtener todos los horarios paginados
        public async Task<List<HorarioDTO>?> ObtenerHorarios(int pagina, int tamanoPagina)
        {
            var horarios = await _repositorioHorario.ObtenerHorarios(pagina, tamanoPagina);

            if (horarios == null || horarios.Count == 0)
            {
                return null;
            }
            var horariosDTO = new List<HorarioDTO>();

            foreach (var horario in horarios)
            {
                var horarioDTO = new HorarioDTO()
                {
                    Id = horario.Id,
                    Asignatura = horario.Asignatura,
                    Profesor = horario.Profesor,
                    IdLaboratorio = horario.IdLaboratorio,
                    HoraInicio = horario.HoraInicio,
                    HoraFinal = horario.HoraFinal,
                    FechaInicio = horario.FechaInicio ?? new DateTime(),
                    FechaFinal = horario.FechaFinal ?? new DateTime(),
                    Dia = horario.Dia
                };
                horariosDTO.Add(horarioDTO);
            }

            //devolver la lista de horarios
            return horariosDTO;
        }

        // Método para obtener todos los horarios por piso
        public async Task<List<HorarioDTO>?> ObtenerHorariosPorPiso(int piso)
        {
            var horarios = await _repositorioHorario.ObtenerHorarioPorPiso(piso);

            if (horarios == null || horarios.Count == 0)
            {
                return null;
            }
            var horariosDTO = new List<HorarioDTO>();

            foreach (var horario in horarios)
            {
                var horarioDTO = new HorarioDTO()
                {
                    Id = horario.Id,
                    Asignatura = horario.Asignatura,
                    Profesor = horario.Profesor,
                    IdLaboratorio = horario.IdLaboratorio,
                    HoraInicio = horario.HoraInicio,
                    HoraFinal = horario.HoraFinal,
                    FechaInicio = horario.FechaInicio ?? new DateTime(),
                    FechaFinal = horario.FechaFinal ?? new DateTime(),
                    Dia = horario.Dia
                };
                horariosDTO.Add(horarioDTO);
            }

            //devolver la lista de horarios
            return horariosDTO;
        }

        // Método para obtener todos los horarios
        public async Task<List<HorarioDTO>?> ObtenerHorariosTotal()
        {
            var horarios = await _repositorioHorario.ObtenerHorariosTotal();

            if (horarios == null || horarios.Count == 0)
            {
                return null;
            }
            var horariosDTO = new List<HorarioDTO>();

            foreach (var horario in horarios)
            {
                var horarioDTO = new HorarioDTO()
                {
                    Id = horario.Id,
                    Asignatura = horario.Asignatura,
                    Profesor = horario.Profesor,
                    IdLaboratorio = horario.IdLaboratorio,
                    HoraInicio = horario.HoraInicio,
                    HoraFinal = horario.HoraFinal,
                    FechaInicio = horario.FechaInicio ?? new DateTime(),
                    FechaFinal = horario.FechaFinal ?? new DateTime(),
                    Dia = horario.Dia
                };
                horariosDTO.Add(horarioDTO);
            }

            //devolver la lista de horarios
            return horariosDTO;
        }

        // Método para obtener un horario por id
        public async Task<HorarioDTO?> ObtenerHorarioPorId(int id)
        {
            var horario = await _repositorioHorario.ObtenerHorarioPorId(id);
            if (horario == null)
            {
                return null;
            }
            var horarioDTO = new HorarioDTO()
            {
                Id = horario.Id,
                Asignatura = horario.Asignatura,
                Profesor = horario.Profesor,
                IdLaboratorio = horario.IdLaboratorio,
                HoraInicio = horario.HoraInicio,
                HoraFinal = horario.HoraFinal,
                FechaInicio = horario.FechaInicio,
                FechaFinal = horario.FechaFinal,
                Dia = horario.Dia
            };
            return horarioDTO;
        }

        // Método para agregar un nuevo horario
        public async Task<(bool Exito, List<HorarioErrores> Errores)> CrearHorariosDesdeLista(List<CrearHorarioDTO> listaHorarios)
        {
            var resultado = await _repositorioHorario.AgregarHorariosAsync(listaHorarios);
            return resultado;
        }

        // Método para actualizar un horario
        public async Task<ActualizarHorarioDTO?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO)
        {
            var horario = await _repositorioHorario.ActualizarHorario(id, actualizarHorarioDTO);
            if (horario == null)
            {
                return null;
            }
            var horarioDTO = new ActualizarHorarioDTO
            {
                Asignatura = horario.Asignatura,
                IdLaboratorio = horario.IdLaboratorio,
                Profesor = horario.Profesor,
                HoraInicio = horario.HoraInicio,
                HoraFinal = horario.HoraFinal,
                FechaInicio = horario.FechaInicio,
                FechaFinal = horario.FechaFinal,
                Dia = horario.Dia
            };
            return horarioDTO;
        }

        // Método para eliminar un horario
        public async Task<bool?> BorrarHorario(int id)
        {
            var horario = await _repositorioHorario.BorrarHorario(id);
            if (horario == null)
            {
                return null;
            }
            return horario;
        }

        // Método para eliminar un horario automatico
        public async Task<bool?> BorrarHorarioAutomatico(bool eliminar)
        {
            var horario = await _repositorioHorario.BorrarHorarioAutomatico(eliminar);
            if (horario == null)
            {
                return null;
            }
            return horario;
        }
    }
}
