namespace Domain.Entities
{
    public class Notifications : BaseEntity
    {
        public string Remarks { get; set; }
        public Guid RoleId { get; set; }
        public string RaisedBy { get; set; }
        public Guid UserId { get; set; }
    }
}
