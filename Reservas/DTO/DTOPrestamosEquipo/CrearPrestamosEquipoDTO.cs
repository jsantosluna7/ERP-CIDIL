namespace Reservas.DTO.DTOPrestamosEquipo
{
    public class CrearPrestamosEquipoDTO
    {

        public int IdUsuario { get; set; }

        public int IdInventario { get; set; }

        public int IdEstado { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinal { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public int? IdUsuarioAprobador { get; set; }

        public string Motivo { get; set; } = null!;

        public string? ComentarioAprobacion { get; set; }
        public int? Cantidad { get; set; }
    }
}
