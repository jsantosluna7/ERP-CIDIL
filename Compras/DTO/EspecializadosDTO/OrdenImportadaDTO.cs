namespace Compras.DTO.EspecializadosDTO
{
    public class OrdenImportadaDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string UnidadNegocio { get; set; } = string.Empty;
        public string SolicitadoPor { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        // ... otros campos
        public List<OrdenItemImportadoDTO> Items { get; set; } = new();
    }
}
