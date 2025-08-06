using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.MPP;

namespace MilkMatrix.Milk.Contracts.Milk.MilkCollection
{
    public interface IMilkCollectionService
    {
        Task InsertMilkCollection(MilkCollectionInsertRequest request);
        Task UpdateMilkCollection(MilkCollectionUpdateRequest request);
        Task DeleteMilkCollection(int id, int userId);
        Task<MilkCollectionResponse?> GetMilkCollectionById(int id);
        Task<IListsResponse<MilkCollectionResponse>> GetMilkCollectionAll(IListsRequest request);
    }
}
