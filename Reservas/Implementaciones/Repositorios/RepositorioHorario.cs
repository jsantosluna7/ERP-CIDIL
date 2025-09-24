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

        //Método para obtener todas las solicitudes de reserva
        public async Task<List<Horario>> ObtenerHorarioPorPiso(int piso)
        {
            try
            {
                // Obtener IDs de laboratorios que pertenecen al piso
                var idsLaboratorios = await _context.Laboratorios
                    .Where(p => p.Piso == piso)
                    .Select(l => l.Id)
                    .ToListAsync();

                // Obtener todas los horarios de esos laboratorios
                var horario = await _context.Horarios
                    .Where(r => idsLaboratorios.Contains(r.IdLaboratorio))
                    .ToListAsync();

                return horario;
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al obtener las solicitudes", ex);
            }
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

        public async Task<(bool Exito, List<HorarioErrores> Errores)> AgregarHorariosAsync(List<CrearHorarioDTO> crearHorariosDTO)
        {
            var nuevos = new List<Horario>();
            var errores = new List<HorarioErrores>();

            bool EsHoraValida(string hora, out TimeSpan ts)
            {
                ts = TimeSpan.Zero;
                return TimeSpan.TryParse(hora, out ts) && ts < TimeSpan.FromHours(24);
            }

            for (int i = 0; i < crearHorariosDTO.Count; i++)
            {
                var dtoA = crearHorariosDTO[i];
                var laboratorioA = await _context.Laboratorios.FindAsync(dtoA.IdLaboratorio);

                if (!EsHoraValida(dtoA.HoraInicio, out var hiA) || !EsHoraValida(dtoA.HoraFinal, out var hfA))
                {
                    errores.Add(new HorarioErrores
                    {
                        idError = 3,
                        titulo = "Formato hora inválido",
                        labNombre = laboratorioA?.Nombre,
                        asignaturaA = dtoA.Asignatura,
                        diaA = dtoA.Dia,
                        horaInicioA = dtoA.HoraInicio,
                        horaFinalA = dtoA.HoraFinal
                    });
                    continue;
                }

                if (hfA <= hiA)
                {
                    errores.Add(new HorarioErrores
                    {
                        idError = 4,
                        titulo = "Hora final debe ser mayor a hora inicial",
                        labNombre = laboratorioA?.Nombre,
                        asignaturaA = dtoA.Asignatura,
                        diaA = dtoA.Dia,
                        horaInicioA = dtoA.HoraInicio,
                        horaFinalA = dtoA.HoraFinal
                    });
                    continue;
                }

                if (hiA < TimeSpan.FromHours(7) || hfA > TimeSpan.FromHours(22))
                {
                    errores.Add(new HorarioErrores
                    {
                        idError = 5,
                        titulo = "Hora fuera de rango permitido",
                        labNombre = laboratorioA?.Nombre,
                        asignaturaA = dtoA.Asignatura,
                        diaA = dtoA.Dia,
                        horaInicioA = dtoA.HoraInicio,
                        horaFinalA = dtoA.HoraFinal
                    });
                    continue;
                }

                // Validación entre DTOs
                for (int j = i + 1; j < crearHorariosDTO.Count; j++)
                {
                    var dtoB = crearHorariosDTO[j];
                    if (dtoA.IdLaboratorio == dtoB.IdLaboratorio && dtoA.Dia == dtoB.Dia)
                    {
                        if (EsHoraValida(dtoB.HoraInicio, out var hiB) && EsHoraValida(dtoB.HoraFinal, out var hfB))
                        {
                            // Solapamiento real
                            if (hiA < hfB && hiB < hfA && !(hfA == hiB || hfB == hiA))
                            {
                                errores.Add(new HorarioErrores
                                {
                                    idError = 1,
                                    titulo = "(En el archivo cargado)",
                                    labNombre = laboratorioA?.Nombre,
                                    asignaturaA = dtoA.Asignatura,
                                    diaA = dtoA.Dia,
                                    horaInicioA = dtoA.HoraInicio,
                                    horaFinalA = dtoA.HoraFinal,
                                    asignaturaB = dtoB.Asignatura,
                                    diaB = dtoB.Dia,
                                    horaInicioB = dtoB.HoraInicio,
                                    horaFinalB = dtoB.HoraFinal
                                });
                            }
                        }
                    }
                }

                // Validación contra DB
                var horariosExistentes = await _context.Horarios
                    .Where(h => h.IdLaboratorio == dtoA.IdLaboratorio && h.Dia == dtoA.Dia)
                    .ToListAsync();

                foreach (var h in horariosExistentes)
                {
                    if (hiA < h.HoraFinal && h.HoraInicio < hfA && !(hfA == h.HoraInicio || h.HoraFinal == hiA))
                    {
                        errores.Add(new HorarioErrores
                        {
                            idError = 2,
                            titulo = "(En el horario)",
                            labNombre = laboratorioA?.Nombre,
                            asignaturaA = dtoA.Asignatura,
                            diaA = dtoA.Dia,
                            horaInicioA = dtoA.HoraInicio,
                            horaFinalA = dtoA.HoraFinal,
                            asignaturaB = h.Asignatura,
                            diaB = h.Dia,
                            horaInicioB = h.HoraInicio?.ToString(@"hh\:mm") ?? "",
                            horaFinalB = h.HoraFinal?.ToString(@"hh\:mm") ?? ""
                        });
                    }
                }

                nuevos.Add(new Horario
                {
                    Asignatura = dtoA.Asignatura,
                    Profesor = dtoA.Profesor,
                    IdLaboratorio = dtoA.IdLaboratorio.Value,
                    Dia = dtoA.Dia,
                    FechaInicio = dtoA.FechaInicio.Date + hiA,
                    FechaFinal = dtoA.FechaFinal.Date + hfA,
                    HoraInicio = hiA,
                    HoraFinal = hfA,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true,
                    ActivadoHorario = true
                });
            }

            if (errores.Any())
                return (false, errores);

            _context.Horarios.AddRange(nuevos);
            await _context.SaveChangesAsync();

            return (true, new List<HorarioErrores>());
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
                var horariosExpirados = await _context.Horarios.Where(h => h.FechaFinal < fechaLimite).ToListAsync();

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
