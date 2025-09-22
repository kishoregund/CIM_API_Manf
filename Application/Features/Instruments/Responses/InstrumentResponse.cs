using Application.Features.Instruments.Requests;

namespace Application.Features.Instruments.Responses
{
    public class InstrumentResponse
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public Guid BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public Guid ManufId { get; set; }
        public string ManufName { get; set; }
        public string SerialNos { get; set; }
        public string InsMfgDt { get; set; }
        public string InsType { get; set; }
        public string InsTypeName { get; set; }
        public string InsVersion { get; set; }
        public string Image { get; set; }

        public List<InstrumentSparesResponse> Spares { get; set; }
        public List<InstrumentAccessoryResponse> Accessories { get; set; }

    }
}
