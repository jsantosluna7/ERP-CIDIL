using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Reservas.Implementaciones.Servicios
{
    public class ServicioConflictos
    {
        private readonly DbErpContext _context;

        public ServicioConflictos(DbErpContext context)
        {
            _context = context;
        }

        public async Task<bool> conflictoReserva(int IdLaboratorio, TimeSpan? HoraInicio, TimeSpan? HoraFinal, DateTime? FechaSolicitud)
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

            // Conversión a DateTimeKind.Utc
            var fecha = FechaSolicitud?.Date;
            var horaInicio = HoraInicio;
            var horaFinal = HoraFinal;

            bool conflictoHorario = await _context.Horarios
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < horaFinal &&
                               h.HoraFinal > horaInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == fecha &&
                               r.HoraInicio < horaFinal &&
                               r.HoraFinal > horaInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }

        public async Task<bool> conflictoReservaActualizar(int id, int IdLaboratorio, TimeSpan? HoraInicio, TimeSpan? HoraFinal, DateTime? FechaSolicitud)
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

            // Conversión a DateTimeKind.Utc
            var fecha = FechaSolicitud?.Date;
            var horaInicio = HoraInicio;
            var horaFinal = HoraFinal;


            bool conflictoHorario = await _context.Horarios
                .AnyAsync(h => h.IdLaboratorio == IdLaboratorio &&
                               h.Dia == dia &&
                               h.HoraInicio < horaFinal &&
                               h.HoraFinal > horaInicio);

            bool conflictoReserva = await _context.ReservaDeEspacios
                .Where(r => r.Id != id) // Excluir la reserva actual
                .AnyAsync(r => r.IdLaboratorio == IdLaboratorio &&
                               r.FechaSolicitud == fecha &&
                               r.HoraInicio < horaFinal &&
                               r.HoraFinal > horaInicio);

            return !(conflictoHorario || conflictoReserva); // Si no hay conflicto, se puede crear la reserva
        }
    }
}
