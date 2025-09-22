using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Masters.Responses
{
    public class ScreensForRolePermissionsResponse : VW_ListTypeItemsResponse
    {
        public string CategoryName { get; set; }
        public Guid Category { get; set; }
    }
}
