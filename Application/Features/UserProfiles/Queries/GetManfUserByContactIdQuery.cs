using Application.Features.Distributors;
using Application.Features.Distributors.Responses;
using Application.Features.UserProfiles.Responses;

namespace Application.Features.UserProfiles.Queries
{
    public class GetManfUserByContactIdQuery : IRequest<IResponseWrapper>
    {
        public Guid ContactId { get; set; }
    }


    public class GetManfUserByContactIdQueryHandler(IUserProfilesService userProfilesService ) : IRequestHandler<GetManfUserByContactIdQuery, IResponseWrapper>
    {
        public async Task<IResponseWrapper> Handle(GetManfUserByContactIdQuery request, CancellationToken cancellationToken)
        {
            var regionContactInDb = (await userProfilesService.GetManfUserByContactAsync(request.ContactId)).Adapt<UserByContactResponse>();

            if (regionContactInDb is not null)
            {
                return await ResponseWrapper<UserByContactResponse>.SuccessAsync(data: regionContactInDb);
            }
            return await ResponseWrapper<UserByContactResponse>.SuccessAsync(message: "Users does not exists.");
        }
    }
}