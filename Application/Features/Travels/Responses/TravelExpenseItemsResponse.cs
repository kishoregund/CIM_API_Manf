namespace Application.Features.AppBasic.Responses
{
    public class TravelExpenseItemsResponse
    {
        public Guid Id { get; set; }
        public Guid TravelExpenseId { get; set; }
        public string ExpDate { get; set; }
        public string ExpDetails { get; set; }
        public bool IsBillsAttached { get; set; }
        public Guid ExpNature { get; set; }
        public string ExpNatureName { get; set; }
        public Guid Currency { get; set; }
        public string CurrencyName { get; set; }
        public Guid ExpenseBy { get; set; }
        public string ExpenseByName { get; set; }
        public decimal BcyAmt { get; set; }
        public decimal UsdAmt { get; set; }
        public string Remarks { get; set; }
        public bool Uploaded { get; set; }
        public string DevRemarks { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}