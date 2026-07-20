namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionDTO
    {
        public string business_unit { get; set; } = string.Empty;
        public string currency { get; set; } = string.Empty;
        public string entered_date { get; set; } = string.Empty;
        public string header_comments { get; set; } = string.Empty;
        public int items_count { get; set; }
        public List<RequisicionLineaDTO> lines { get; set; } = new();
        public string requested_by { get; set; } = string.Empty;
        public string requester_id { get; set; } = string.Empty;
        public string requisition_id { get; set; } = string.Empty;
        public string requisition_name { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}
