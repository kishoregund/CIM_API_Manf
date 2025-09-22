namespace Domain.Entities
{
    public class AMCItems : BaseEntity
    {
        public int SqNo { get; set; }
        public Guid AMCId { get; set; }
        public string ServiceType { get; set; }
        public string Date { get; set; }
        public string ServiceRequestId { get; set; }
        public string EstStartDate { get; set; }
        public string EstEndDate { get; set; }
        public string Status { get; set; }
    }
}
