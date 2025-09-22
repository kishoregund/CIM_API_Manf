using Application.Features.Distributors;
using Application.Features.Distributors.Responses;
using Application.Features.UserProfiles.Responses;

namespace Application.Features.UserProfiles.Queries
{
    public class GetCustUserByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetCustUserByContactIdQueryHandler(IUserProfilesService userProfilesService ) : IRequestHandler<GetCustUserByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetCustUserByContactIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await userProfilesService.GetCustUserByContactAsync(request.ContactId)).Adapt<UserByContactResponse>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<UserByContactResponse>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<UserByContactResponse>.SuccessAsync(message: "Users does not exists.");
        }
    }
}