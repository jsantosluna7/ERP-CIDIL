using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOHorario;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioHorario : IRepositorioHorario
    {
        private readonly DbErpContext _context;
        public RepositorioHorario(DbErpContext context)
        {
            _context = context;
        }

        // Método para obtener todos los horarios
        public async Task<List<Horario>> ObtenerHorarios()
        {
            return await _context.Horarios.Where(h => h.ActivadoHorario == true).ToListAsync();
        }

        //Método para obtener un horario por id
        public async Task<Horario?> ObtenerHorarioPorId(int id)
        {
            var horario = await _context.Horarios.Include(h => h.IdLaboratorioNavigation).FirstOrDefaultAsync(h => h.Id == id);

            if (horario == null)
            {
                return null;
            }

            return horario;
        }

        // Método para crear un nuevo horario
        public async Task<Horario?> AgregarHorario(CrearHorarioDTO crearHorarioDTO)
        {
            //verificar si el horario ya existe
            var horarioExitesente = await _context.Horarios
                .AnyAsync(h => h.Asignatura == crearHorarioDTO.Asignatura &&
                               h.Profesor == crearHorarioDTO.Profesor &&
                               h.IdLaboratorio == crearHorarioDTO.IdLaboratorio &&
                               h.HoraInicio == crearHorarioDTO.HoraInicio &&
                               h.HoraFinal == crearHorarioDTO.HoraFinal &&
                               h.Dia == crearHorarioDTO.Dia);
            if (horarioExitesente)
            {
                return null;
            }

            // Crear una nueva instancia de Horario
            var nuevoHorario = new Horario
            {
                Asignatura = crearHorarioDTO.Asignatura,
                Profesor = crearHorarioDTO.Profesor,
                IdLaboratorio = crearHorarioDTO.IdLaboratorio,
                HoraInicio = crearHorarioDTO.HoraInicio,
                HoraFinal = crearHorarioDTO.HoraFinal,
                Dia = crearHorarioDTO.Dia
            };

            //Guardar el nuevo horario en la base de datos
            _context.Horarios.Add(nuevoHorario);
            await _context.SaveChangesAsync();

            // Devolver el nuevo horario
            return nuevoHorario;
        }

        // Método para actualizar un horario existente
        public async Task<Horario?> ActualizarHorario(int id, ActualizarHorarioDTO actualizarHorarioDTO)
        {
            // Buscar el horario existente por su ID
            var horarioExistente = await ObtenerHorarioPorId(id);

            // Verificar si el horario existe
            if (horarioExistente == null)
            {
                return null; // El horario no existe
            }

            // Actualizar las propiedades del horario existente
            horarioExistente.Asignatura = actualizarHorarioDTO.Asignatura ?? horarioExistente.Asignatura;
            horarioExistente.Profesor = actualizarHorarioDTO.Profesor ?? horarioExistente.Profesor;
            horarioExistente.IdLaboratorio = actualizarHorarioDTO.IdLaboratorio ?? horarioExistente.IdLaboratorio;
            horarioExistente.HoraInicio = actualizarHorarioDTO.HoraInicio ?? horarioExistente.HoraInicio;
            horarioExistente.HoraFinal = actualizarHorarioDTO.HoraFinal ?? horarioExistente.HoraFinal;
            horarioExistente.Dia = actualizarHorarioDTO.Dia ?? horarioExistente.Dia;

            // Guardar los cambios en la base de datos
            _context.Horarios.Update(horarioExistente);
            await _context.SaveChangesAsync();

            var horarioActualizado = await ObtenerHorarioPorId(id);
            return horarioActualizado; // Devolver el horario actualizado
        }

        // Método para eliminar un horario por su ID
        public async Task<bool?> BorrarHorario(int id)
        {
            var horario = await ObtenerHorarioPorId(id);
            if (horario == null)
            {
                return null; // El horario no existe
            }

            // Eliminar el horario de la base de datos
            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();
            return true; // El horario se eliminó correctamente
        }

        // Método para desactivar el horario despues de 4 meses
        public async Task<bool?> BorrarHorarioAutomatico(bool eliminar)
        {
                var fechaLimite = DateTime.UtcNow.AddDays(-1);
                var horariosExpirados = await _context.Horarios.Where(h => h.FechaCreacion < fechaLimite).ToListAsync();

                if (horariosExpirados.Count > 0)
                {
                    try
                    {
                        foreach(Horario horario in horariosExpirados)
                        {
                            horario.ActivadoHorario = false;
                            _context.Update(horario);
                            await _context.SaveChangesAsync();
                        }
                        return true; //decir que se desactivaron los horarios
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción según sea necesario
                        Console.WriteLine($"Error al eliminar horarios: {ex.Message}");
                        return false; //decir que no se pudo eliminar los horarios
                    }
                }
                return null;
        }
    }
}
