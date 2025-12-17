namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionShipmentDTO
    {
        public string attention { get; set; }
        public string ship_to { get; set; }
        public string ship_via { get; set; }
        public decimal shipping_quantity { get; set; }
    }
}
