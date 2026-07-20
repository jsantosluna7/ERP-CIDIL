namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionLineaDTO
    {
        public string item_description { get; set; } = string.Empty;
        public string line_comments { get; set; } = string.Empty;
        public int line_number { get; set; }
        public string line_status { get; set; } = string.Empty;
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public string unit_of_measure { get; set; } = string.Empty;
        public List<RequisicionShipmentDTO> shipments { get; set; } = new();
    }
}
