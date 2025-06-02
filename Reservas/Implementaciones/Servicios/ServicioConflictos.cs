using Microsoft.EntityFrameworkCore;
using Reservas.Modelos;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioConflictos
    {
        private readonly DbErpContext _context;

        public ServicioConflictos(DbErpContext context)
        {
            _context = context;
        }

        public async Task<bool> conflictoReserva(int IdLaboratorio, DateTime? HoraInicio, DateTime? HoraFinal, DateTime? FechaSolicitud)
        {
            var diaSemana = FechaSolicitud?.DayOfWeek.ToString();
            var dia = diaSemana switch
            {
                "Monday" => "Lunes",
                "Tuesday" => "Martes",
                "Wednesday" => "Miércoles",
                "Thursday" => "Jueves",
                "Friday" => "Viernes",
                "Saturday" => "Sábado",
                "Sunday" => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };


            bool conflictoHorario = await _context.Horarios
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < HoraFinal &&
                               h.HoraFinal > HoraInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == FechaSolicitud &&
                               r.HoraInicio < HoraFinal &&
                               r.HoraFinal > HoraInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }

        public async Task<bool> conflictoReservaActualizar(int id, int IdLaboratorio, DateTime? HoraInicio, DateTime? HoraFinal, DateTime? FechaSolicitud)
        {
            var diaSemana = FechaSolicitud?.DayOfWeek.ToString();
            var dia = diaSemana switch
            {
                "Monday" => "Lunes",
                "Tuesday" => "Martes",
                "Wednesday" => "Miércoles",
                "Thursday" => "Jueves",
                "Friday" => "Viernes",
                "Saturday" => "Sábado",
                "Sunday" => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };


            bool conflictoHorario = await _context.Horarios
                .Where(h => h.IdLaboratorio != IdLaboratorio) // Excluir la reserva actual
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < HoraFinal &&
                               h.HoraFinal > HoraInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .Where(r => r.Id != id) // Excluir la reserva actual
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == FechaSolicitud &&
                               r.HoraInicio < HoraFinal &&
                               r.HoraFinal > HoraInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }
    }
}
