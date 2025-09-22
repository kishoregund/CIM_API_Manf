namespace Application.Features.AMCS.Requests
{
    public class AmcItemsRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public int SqNo { get; set; }
        public Guid AMCId { get; set; }
        public string ServiceType { get; set; }
        [SkipGlobalValidation]
        public string Date { get; set; }
        [SkipGlobalValidation]
        public string ServiceRequestId { get; set; }
        [SkipGlobalValidation]
        public string EstStartDate { get; set; }
        [SkipGlobalValidation]
        public string EstEndDate { get; set; }
        [SkipGlobalValidation]
        public string Status { get; set; }
    }
}
