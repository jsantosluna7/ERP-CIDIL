using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOHorario;

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
        public async Task<List<Horario>> ObtenerHorarios(int pagina, int tamanoPagina)
        {
            if (pagina <= 0) pagina = 1;
            if (tamanoPagina <= 0) tamanoPagina = 20;

            return await _context.Horarios
                .Where(h => h.ActivadoHorario == true)
                .OrderBy(i => i.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();
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
        public async Task<(bool Exito, List<string> Errores)> AgregarHorariosAsync(List<CrearHorarioDTO> crearHorariosDTO)
        {
            var errores = new List<string>();

            // 1. Validar conflictos contra la base de datos
            for (int i = 0; i < crearHorariosDTO.Count; i++)
            {
                var dto = crearHorariosDTO[i];
                var hayConflictoBD = await _context.Horarios.AnyAsync(h =>
                    h.IdLaboratorio == dto.IdLaboratorio &&
                    h.Dia == dto.Dia &&
                    (
                        (dto.HoraInicio >= h.HoraInicio && dto.HoraInicio < h.HoraFinal) ||
                        (dto.HoraFinal > h.HoraInicio && dto.HoraFinal <= h.HoraFinal) ||
                        (dto.HoraInicio <= h.HoraInicio && dto.HoraFinal >= h.HoraFinal)
                    )
                );

                var baseDatos = await _context.Horarios.Where(h =>
                    h.IdLaboratorio == dto.IdLaboratorio &&
                    h.Dia == dto.Dia &&
                    (
                        (dto.HoraInicio >= h.HoraInicio && dto.HoraInicio < h.HoraFinal) ||
                        (dto.HoraFinal > h.HoraInicio && dto.HoraFinal <= h.HoraFinal) ||
                        (dto.HoraInicio <= h.HoraInicio && dto.HoraFinal >= h.HoraFinal)
                    )
                ).FirstOrDefaultAsync();


                var laboratorio = await _context.Laboratorios.Where(l => l.Id == dto.IdLaboratorio).FirstOrDefaultAsync();

                if (hayConflictoBD)
                {
                    errores.Add($"Conflicto con horario existente en BD con las materias {baseDatos.Asignatura} y {dto.Asignatura}, Lab {laboratorio.CodigoDeLab}, Día {dto.Dia}, de {dto.HoraInicio:t} a {dto.HoraFinal:t}");
                }
            }

            // 2. Validar conflictos internos entre los elementos de la misma lista
            for (int i = 0; i < crearHorariosDTO.Count; i++)
            {
                for (int j = i + 1; j < crearHorariosDTO.Count; j++)
                {
                    var h1 = crearHorariosDTO[i];
                    var h2 = crearHorariosDTO[j];
                    var laboratorio = await _context.Laboratorios.Where(l => l.Id == h1.IdLaboratorio).FirstOrDefaultAsync();

                    if (h1.IdLaboratorio == h2.IdLaboratorio && h1.Dia == h2.Dia)
                    {
                        bool solapan = h1.HoraInicio < h2.HoraFinal && h2.HoraInicio < h1.HoraFinal;
                        if (solapan)
                        {
                            errores.Add($"Conflicto con las materias {h1.Asignatura} y {h2.Asignatura}: se solapan en Lab {laboratorio.CodigoDeLab} en las horas {h1.HoraInicio} - {h1.HoraFinal} y {h2.HoraInicio} - {h2.HoraFinal}en el Día {h1.Dia}");
                        }
                    }
                }
            }

            if (errores.Any())
            {
                return (false, errores);
            }

            var nuevos = crearHorariosDTO.Select(dto => new Horario
            {
                Asignatura = dto.Asignatura,
                Profesor = dto.Profesor,
                IdLaboratorio = dto.IdLaboratorio,
                HoraInicio = dto.HoraInicio,
                HoraFinal = dto.HoraFinal,
                Dia = dto.Dia
            }).ToList();

            await _context.Horarios.AddRangeAsync(nuevos);
            await _context.SaveChangesAsync();

            return (true, new List<string>());
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
