namespace Application.Features.Travels.Responses
{
    public class AdvanceRequestResponse
    {
        public Guid Id { get; set; }
        public Guid EngineerId { get; set; }
        public string EngineerName { get; set; }
        public Guid DistributorId { get; set; }
        public string DistributorName { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string ServiceRequestNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool UnderTaking { get; set; }
        public bool IsBillable { get; set; }
        public decimal AdvanceAmount { get; set; }
        public Guid AdvanceCurrency { get; set; }
        public string ReportingManager { get; set; }
        public string ClientNameLocation { get; set; }
        public Guid OfficeLocationId { get; set; }
        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } 
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
