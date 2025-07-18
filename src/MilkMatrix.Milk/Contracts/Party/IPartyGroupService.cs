using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Party;
using MilkMatrix.Milk.Models.Response.Party;

namespace MilkMatrix.Milk.Contracts.Party
{
    public interface IPartyGroupService
    {
        Task<IListsResponse<PartyGroupResponse>> GetAll(IListsRequest request);
        Task<PartyGroupResponse?> GetById(long id);
        Task<IEnumerable<CommonLists>> GetSpecificLists(PartyGroupRequest request);
        Task<IEnumerable<PartyGroupResponse>> GetPartyGroups(PartyGroupRequest request);
        Task AddPartyGroup(PartyGroupInsertRequest request);
        Task UpdatePartyGroup(PartyGroupUpdateRequest request);
        Task DeleteById(long id, long userId);
    }
}
