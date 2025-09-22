namespace Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }
        public Guid BusinessUnitId { get; set; }
    }
}