namespace Application.Features.Identity.Tokens
{
    public class TokenRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [SkipGlobalValidation]
        public string BusinessUnitId{ get; set; }
        [SkipGlobalValidation]
        public string BrandId { get; set; }
    }
}
