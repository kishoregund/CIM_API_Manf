using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BankDetails : BaseEntity
    {
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string NameInBank { get; set; }
        public string Branch { get; set; }
        public string IBANNo { get; set; }
        public string BankSwiftCode { get; set; }
        public Guid ContactId { get; set; }
    }
}
