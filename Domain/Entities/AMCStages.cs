namespace Domain.Entities
{
    public class AMCStages : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public Guid AMCId { get; set; }
        public int StageIndex { get; set; }
        public string Stage { get; set; }
        public int? Index { get; set; }
        public bool IsCompleted { get; set; }
        public string PaymentTypeId { get; set; }
        public string PayAmtCurrencyId { get; set; }
        public double PayAmt { get; set; }
    }
}
