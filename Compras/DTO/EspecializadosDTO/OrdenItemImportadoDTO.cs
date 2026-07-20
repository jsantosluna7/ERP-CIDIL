namespace Compras.DTO.EspecializadosDTO
{
    public class OrdenItemImportadoDTO
    {
        public string NumeroLista { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        // puedes agregar más campos: precio, UOM, comentarios...
    }
}
