namespace Application.Features.AMCS.Responses
{
    public class AmcStagesResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public Guid AMCId { get; set; }
        public int StageIndex { get; set; }
        public string Stage { get; set; }
        public string StageName { get; set; }
        public int? Index { get; set; }
        public bool IsCompleted { get; set; }
        public string PaymentTypeId { get; set; }
        public string PayAmtCurrencyId { get; set; }
        public double PayAmt { get; set; }
    }
}
