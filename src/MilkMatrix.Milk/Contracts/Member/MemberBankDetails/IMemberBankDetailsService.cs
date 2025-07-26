using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Response.Member.MemberBankDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Member.MemberBankDetails
{
    public interface IMemberBankDetailsService
    {
        Task AddMemberBankDetails(MemberBankDetailsInsertRequest request);
        Task UpdateMemberBankDetails(MemberBankDetailsUpdateRequest request);
        Task DeleteMemberBankDetails(long bankDetailId, long userId);
        Task<MemberBankDetailsResponse?> GetMemberBankDetailsById(long id);
        Task<IEnumerable<MemberBankDetailsResponse>> GetMemberBankDetails(MemberBankDetailsRequestModel request);
        Task<IListsResponse<MemberBankDetailsResponse>> GetAllMemberBankDetails(IListsRequest request);
    }
}
