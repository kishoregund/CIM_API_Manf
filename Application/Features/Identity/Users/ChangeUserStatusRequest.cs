namespace Application.Features.Identity.Users
{
    public class ChangeUserStatusRequest
    {
        //public string UserId { get; set; }
        public string Email { get; set; }
        public string ContactType { get; set; }
        public string ContactId { get; set; }
        public bool Activation { get; set; }
    }
}
