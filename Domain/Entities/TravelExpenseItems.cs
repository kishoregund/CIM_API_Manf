using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TravelExpenseItems : BaseEntity
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
    }
}
