using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Party;
using MilkMatrix.Milk.Models.Response.Party;

namespace MilkMatrix.Milk.Contracts.Party
{
    public interface IPartyService
    {
        Task<IListsResponse<PartyResponse>> GetAll(IListsRequest request);
        Task<PartyResponse?> GetById(long id);
        Task<IEnumerable<PartyResponse>> GetParties(PartyRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(PartyRequest request);
        Task AddParty(PartyInsertRequest request);
        Task UpdateParty(PartyUpdateRequest request);
        Task Delete(long id, long userId);
    }
}
