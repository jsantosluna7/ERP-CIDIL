using Inventario.DTO.InventarioEquipoDTO;

namespace Reservas.DTO.DTOSolicitudDeEquipos
{
    public class SolicitudPrestamosDeEquiposDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdInventario { get; set; }
        public InventarioEquipoDTO Inventario { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Motivo { get; set; } = null!;
        public DateTime? FechaSolicitud { get; set; }
        public int? IdEstado { get; set; }
        public int? Cantidad { get; set; }
    }

    public class CrearSolicitudPrestamosDeEquiposDTO
    {
        public int IdUsuario { get; set; }
        public DateTime FechaSolicitud { get; set; }
        // Lista de equipos para resolver el problema de múltiples llamadas del frontend
        public List<CrearSolicitudEquipoItemDTO> Equipos { get; set; } = new();
    }

    public class CrearSolicitudEquipoItemDTO
    {
        public int IdInventario { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }

    public class ActualizarSolicitudPrestamosDeEquiposDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Motivo { get; set; } = null!;
        public int Cantidad { get; set; }
    }

    // Resultado por cada equipo individual
    public class ResultadoItemSolicitudDTO
    {
        public int IdInventario { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public bool Exitoso { get; set; }
        public string? Error { get; set; }
        public SolicitudPrestamosDeEquiposDTO? Solicitud { get; set; }
    }

    // Respuesta completa del POST
    public class ResultadoCrearMultiplesDTO
    {
        public bool TodosExitosos { get; set; }
        public List<ResultadoItemSolicitudDTO> Resultados { get; set; } = new();
    }
}
