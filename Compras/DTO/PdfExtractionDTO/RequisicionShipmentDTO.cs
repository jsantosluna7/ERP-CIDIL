namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionShipmentDTO
    {
        public string attention { get; set; } = string.Empty;
        public string ship_to { get; set; } = string.Empty;
        public string ship_via { get; set; } = string.Empty;
        public decimal shipping_quantity { get; set; }
    }
}
