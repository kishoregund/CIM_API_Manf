namespace Application.Features.Travels.Requests
{
    public class TravelExpenseItemsRequest
    {
        public Guid TravelExpenseId { get; set; }
        public string ExpDate { get; set; }
        public string ExpDetails { get; set; }
        public Guid ExpNature { get; set; }
        public bool IsBillsAttached { get; set; }
        public Guid Currency { get; set; }
        public Guid ExpenseBy { get; set; }
        public decimal BcyAmt { get; set; }
        public decimal UsdAmt { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;

    }

    public class UpdateTravelExpenseItemsRequest
    {
        public Guid Id { get; set; }
        public Guid TravelExpenseId { get; set; }
        public string ExpDate { get; set; }
        public string ExpDetails { get; set; }
        public Guid ExpNature { get; set; }
        public bool IsBillsAttached { get; set; }
        public Guid Currency { get; set; }
        public Guid ExpenseBy { get; set; }
        public decimal BcyAmt { get; set; }
        public decimal UsdAmt { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
    }
}
