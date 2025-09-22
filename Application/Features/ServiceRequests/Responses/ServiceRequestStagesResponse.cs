using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests.Responses
{
    public class ServiceRequestStagesResponse
    {
        public int Id { get; set; }
        public bool Created { get; set; }
        public bool Assigned { get; set; }
        public bool MeetingScheduled { get; set; }
        public bool InProgress { get; set; }
        public bool EngSigned { get; set; }
        public bool CustSigned { get; set; }
        public bool Completed { get; set; }
    }
}
