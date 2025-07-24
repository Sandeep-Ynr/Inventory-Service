using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Response.Member;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Member
{
    public interface IMemberService
    {
        Task AddMember(MemberInsertRequest request);
        Task UpdateMember(MemberUpdateRequest request);
        Task Delete(int memberId, int userId);
        Task<MemberResponse?> GetById(int id);
        Task<IEnumerable<MemberResponse>> GetMembers(MemberRequestModel request);
        Task<IListsResponse<MemberResponse>> GetAll(IListsRequest request);
    }
}
