using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Masters.Responses
{
    public class VW_ListTypeItemsResponse
    {
        public Guid ListTypeId { get; set; }
        public Guid ListTypeItemId { get; set; }
        public bool IsEscalationSupervisor { get; set; }
        public bool IsDeleted { get; set; }
        public int IsMaster { get; set; }
        public string ItemCode { get; set; }
        public string ListCode { get; set; }
        public string ListName { get; set; }
        public string ItemName { get; set; }
    }
}
