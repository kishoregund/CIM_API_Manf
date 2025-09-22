using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Views
{
    public class VW_InstrumentSpares
    {
        public Guid Id { get; set; }
        public Guid InstrumentId { get; set; }
        public Guid ConfigTypeId { get; set; }
        public Guid ConfigValueId { get; set; }
        public string ConfigTypeName { get; set; }
        public string ConfigValueName { get; set; }
        public string PartNo { get; set; }
        public string PartNoDesc { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public int InsQty { get; set; }
        public Guid PartType { get; set; }
        public string PartTypeName { get; set; }
        public string DescCatalogue { get; set; }
        public string HsCode { get; set; }
        public Guid CountryId { get; set; }
        public decimal Price { get; set; }
        public Guid CurrencyId { get; set; }
        public string Image { get; set; }
        public bool IsObselete { get; set; }
        public Guid? ReplacePartNoId { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        //public string PartNoDesc { get; set; }
    }
}
