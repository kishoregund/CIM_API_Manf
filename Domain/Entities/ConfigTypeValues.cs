namespace Domain.Entities
{
    public class ConfigTypeValues:BaseEntity
    {
        public Guid ListTypeItemId { get; set; }
        public string ConfigValue { get; set; }
    }
}
