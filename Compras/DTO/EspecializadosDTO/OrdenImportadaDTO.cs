namespace Compras.DTO.EspecializadosDTO
{
    public class OrdenImportadaDTO
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string UnidadNegocio { get; set; }
        public string SolicitadoPor { get; set; }
        public string Moneda { get; set; }
        // ... otros campos
        public List<OrdenItemImportadoDTO> Items { get; set; }
    }
}
