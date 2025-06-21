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

        // Método para obtener todos los horarios paginado
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

        // Método para obtener todos los horarios
        public async Task<List<Horario>> ObtenerHorariosTotal()
        {
            return await _context.Horarios
                .Where(h => h.ActivadoHorario == true)
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

        public async Task<(bool Exito, List<string> Errores)> AgregarHorariosAsync(List<CrearHorarioDTO> crearHorariosDTO)
        {
            var errores = new List<string>();

            // 1. Validación interna entre los DTO antes de la BD
            for (int i = 0; i < crearHorariosDTO.Count; i++)
            {
                var dtoA = crearHorariosDTO[i];
                if (!TimeSpan.TryParse(dtoA.HoraInicio, out var hiA) ||
                    !TimeSpan.TryParse(dtoA.HoraFinal, out var hfA))
                {
                    errores.Add($"Formato de hora inválido en {dtoA.Asignatura}: {dtoA.HoraInicio} - {dtoA.HoraFinal}");
                    continue;
                }

                for (int j = i + 1; j < crearHorariosDTO.Count; j++)
                {
                    var dtoB = crearHorariosDTO[j];
                    if (dtoA.IdLaboratorio == dtoB.IdLaboratorio && dtoA.Dia == dtoB.Dia)
                    {
                        if (!TimeSpan.TryParse(dtoB.HoraInicio, out var hiB) ||
                            !TimeSpan.TryParse(dtoB.HoraFinal, out var hfB))
                        {
                            errores.Add($"Formato de hora inválido en {dtoB.Asignatura}: {dtoB.HoraInicio} - {dtoB.HoraFinal}");
                            continue;
                        }

                        // Comparar rangos de horas
                        bool overlapHoras = hiA < hfB && hfA > hiB;
                        bool overlapFechas = dtoA.FechaInicio < dtoB.FechaFinal && dtoA.FechaFinal > dtoB.FechaInicio;

                        if (overlapHoras && overlapFechas)
                        {
                            errores.Add($"Solapamiento interno: {dtoA.Asignatura} y {dtoB.Asignatura}, Lab {dtoA.IdLaboratorio}, {dtoA.Dia}, horas {dtoA.HoraInicio}-{dtoA.HoraFinal} vs {dtoB.HoraInicio}-{dtoB.HoraFinal}");
                        }
                    }
                }
            }

            if (errores.Any())
                return (false, errores);

            // 2. Si no hay errores internos, seguir con tu validación y DB
            foreach (var dto in crearHorariosDTO)
            {
                if (!TimeSpan.TryParse(dto.HoraInicio, out var horaInicio) ||
                    !TimeSpan.TryParse(dto.HoraFinal, out var horaFinal))
                {
                    errores.Add($"Formato de hora inválido en {dto.Asignatura}: {dto.HoraInicio} - {dto.HoraFinal}");
                    continue;
                }

                var conflictos = await _context.Horarios
                    .Where(h =>
                        h.IdLaboratorio == dto.IdLaboratorio &&
                        h.Dia == dto.Dia &&
                        dto.FechaInicio < h.FechaFinal &&
                        dto.FechaFinal > h.FechaInicio &&
                        horaInicio < h.HoraFinal &&
                        horaFinal > h.HoraInicio
                    ).ToListAsync();

                if (conflictos.Any())
                {
                    var lab = await _context.Laboratorios.FindAsync(dto.IdLaboratorio);
                    foreach (var h in conflictos)
                    {
                        errores.Add($"Conflicto en BD: {dto.Asignatura} vs {h.Asignatura}, Lab {lab?.CodigoDeLab}, {dto.Dia}, fechas {dto.FechaInicio:dd/MM/yyyy}-{dto.FechaFinal:dd/MM/yyyy}, horas {dto.HoraInicio} - {dto.HoraFinal}");
                    }
                    continue;
                }

                var nuevo = new Horario
                {
                    Asignatura = dto.Asignatura,
                    Profesor = dto.Profesor,
                    IdLaboratorio = dto.IdLaboratorio,
                    Dia = dto.Dia,
                    FechaInicio = dto.FechaInicio.Date,
                    FechaFinal = dto.FechaFinal.Date,
                    HoraInicio = horaInicio,
                    HoraFinal = horaFinal,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true,
                    ActivadoHorario = true
                };

                _context.Horarios.Add(nuevo);
            }

            if (errores.Any())
                return (false, errores);

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
            horarioExistente.FechaInicio = actualizarHorarioDTO.FechaInicio ?? horarioExistente.FechaInicio;
            horarioExistente.FechaFinal = actualizarHorarioDTO.FechaFinal ?? horarioExistente.FechaFinal;
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
