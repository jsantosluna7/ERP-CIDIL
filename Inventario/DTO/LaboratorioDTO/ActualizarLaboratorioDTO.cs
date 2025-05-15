namespace Inventario.DTO.LaboratorioDTO
{
    public class ActualizarLaboratorioDTO
    {
       

        public string CodigoDeLab { get; set; } = null!;

        public int? Capacidad { get; set; }

        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }

        public int? Piso { get; set; }

        
    }
}
