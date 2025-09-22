namespace Application.Features.AMCS.Responses
{
    public class AmcItemsResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public int SqNo { get; set; }
        public string AMCId { get; set; }
        public string ServiceType { get; set; }
        public string Date { get; set; }
        public string ServiceRequestId { get; set; }
        public string SerReqNo { get; set; }
        public string EstStartDate { get; set; }
        public string EstEndDate { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
    }
}
