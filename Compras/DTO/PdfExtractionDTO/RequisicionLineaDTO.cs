namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionLineaDTO
    {
        public string item_description { get; set; }
        public string line_comments { get; set; }
        public int line_number { get; set; }
        public string line_status { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }
        public string unit_of_measure { get; set; }
        public List<RequisicionShipmentDTO> shipments { get; set; }
    }
}
