using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Member.MemberAddress;
using MilkMatrix.Milk.Models.Response.Member.MemberAddress;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Member.MemberAddress
{
    public interface IMemberAddressService
    {
        Task AddMemberAddress(MemberAddressInsertRequest request);
        Task UpdateMemberAddress(MemberAddressUpdateRequest request);
        Task DeleteMemberAddress(long addressId, long userId);
        Task<MemberAddressResponse?> GetMemberAddressById(long id);
        Task<IEnumerable<MemberAddressResponse>> GetMemberAddresses(MemberAddressRequestModel request);
        Task<IListsResponse<MemberAddressResponse>> GetAllMemberAddresses(IListsRequest request);
    }
}
