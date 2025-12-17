namespace Compras.DTO.PdfExtractionDTO
{
    public class RequisicionDTO
    {
        public string business_unit { get; set; }
        public string currency { get; set; }
        public string entered_date { get; set; }
        public string header_comments { get; set; }
        public List<RequisicionLineaDTO> lines { get; set; }
        public string requested_by { get; set; }
        public string requester_id { get; set; }
        public string requisition_id { get; set; }
        public string requisition_name { get; set; }
        public decimal requisition_total { get; set; }
        public string status { get; set; }
    }
}
