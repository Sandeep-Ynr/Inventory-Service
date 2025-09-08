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
        Task<PartyDetailResponse?> GetById(long id);
       
        Task AddParty(PartyInsertRequest request);
        Task UpdateParty(long id,PartyUpdateRequest request);
    }
}
