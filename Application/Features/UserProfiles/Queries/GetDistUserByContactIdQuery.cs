using Application.Features.Distributors;
using Application.Features.Distributors.Responses;
using Application.Features.UserProfiles.Responses;

namespace Application.Features.UserProfiles.Queries
{
    public class GetDistUserByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetDistUserByContactIdQueryHandler(IUserProfilesService userProfilesService ) : IRequestHandler<GetDistUserByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetDistUserByContactIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await userProfilesService.GetDistUserByContactAsync(request.ContactId)).Adapt<UserByContactResponse>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<UserByContactResponse>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<UserByContactResponse>.SuccessAsync(message: "Users does not exists.");
        }
    }
}