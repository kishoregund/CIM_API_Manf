using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ServiceRequests.Responses
{
    public class SRInstrumentResponse
    {
        public Guid Id { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
        public Guid CustSiteId { get; set; }
        public string SerialNos { get; set; }
        public string InsType { get; set; }
        public string InsVersion { get; set; }
        public Guid OperatorId { get; set; }
        public Guid InstruEngineerId { get; set; }
        public ContactResponse OperatorEng { get; set; }
        public ContactResponse MachineEng { get; set; }
    }

    public class ContactResponse
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrimaryContactNo { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryContactNo { get; set; }
        public string SecondaryEmail { get; set; }
        public string DesignationId { get; set; }
        public string Designation { get; set; }
    }
}
