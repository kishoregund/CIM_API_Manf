namespace Application.Features.Travels.Responses
{
    public class TravelExpenseResponse
    {
        public Guid Id { get; set; }
        public Guid EngineerId { get; set; }
        public string EngineerName { get; set; }
        public Guid DistributorId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string ServiceRequestNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public Guid Designation { get; set; }
        public decimal GrandCompanyTotal { get; set; }
        public decimal GrandEngineerTotal { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
