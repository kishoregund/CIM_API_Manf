using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboards.Responses
{
    public class InstrumentOwnershipResponse
    {
        public string InstrumentId { get; set; }
        public string InsSerialNo { get; set; }
        public string DateOfPurchase { get; set; }
        public string InsCostCurrency { get; set; }
        public decimal InsCost { get; set; }
        public List<CostOfOwnership> OwnerShip { get; set; }

    }

    public class CostOfOwnership
    {

        public int ServiceContractYrs { get; set; } = 0;
        public decimal ServiceContractCost { get; set; } = 0;
        public string ServiceContractCostCurrencyId { get; set; }
        public decimal BreakdownVisitCost { get; set; } = 0;
        public string BreakdownVisitCostCurrencyId { get; set; }
        public decimal SparePartsCost { get; set; } = 0;
        public string SparePartsCostCurrencyId { get; set; }
        public decimal TotalAnnualCost { get; set; } = 0;
        public decimal TotalCummulativeCost { get; set; } = 0;
        public decimal CostOfOwnershipAnnualy { get; set; } = 0;
        public decimal AverageCostOfOwnershipAnnualy { get; set; } = 0;
    }
}
