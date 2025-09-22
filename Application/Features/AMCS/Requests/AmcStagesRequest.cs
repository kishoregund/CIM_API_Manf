namespace Application.Features.AMCS.Requests
{
    public class AmcStagesRequest
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
        public int? Index { get; set; }
        public bool IsCompleted { get; set; }
        [SkipGlobalValidation]
        public string PaymentTypeId { get; set; }
        [SkipGlobalValidation]
        public string PayAmtCurrencyId { get; set; }
        [SkipGlobalValidation]
        public double PayAmt { get; set; }
    }
}
