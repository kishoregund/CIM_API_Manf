namespace Domain.Entities
{
    public class Instrument : BaseEntity
    {
        public Guid BusinessUnitId { get; set; }
        public Guid BrandId { get; set; }
        public Guid ManufId { get; set; }
        public string SerialNos { get; set; }
        public string InsMfgDt { get; set; }
        public string InsType { get; set; }
        public string InsVersion { get; set; }
        public string Image { get; set; }
    }
}
