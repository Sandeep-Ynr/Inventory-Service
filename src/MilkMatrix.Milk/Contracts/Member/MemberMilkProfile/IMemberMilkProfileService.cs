using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Milk.Models.Response.Member.MemberMilkProfile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Member.MilkProfile
{
    public interface IMemberMilkProfileService
    {
        Task AddMemberMilkProfile(MemberMilkProfileInsertRequest request);
        Task UpdateMemberMilkProfile(MemberMilkProfileUpdateRequest request);
        Task DeleteMemberMilkProfile(long milkProfileId, long userId);
        Task<MemberMilkProfileResponse?> GetMemberMilkProfileById(long id);
        Task<IEnumerable<MemberMilkProfileResponse>> GetMemberMilkProfiles(MemberMilkProfileRequestModel request);
        Task<IListsResponse<MemberMilkProfileResponse>> GetAllMemberMilkProfiles(IListsRequest request);
    }
}
