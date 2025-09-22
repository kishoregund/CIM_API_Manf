
namespace Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string MCId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? N_Code { get; set; }
        public int Minor_Unit { get; set; }
        public string Symbol { get; set; }
    }
}
