using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerCollection;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerCollection;

namespace MilkMatrix.Milk.Contracts.Milk.Transaction.FarmerCollection
{
    public interface IFarmerCollectionService
    {
        Task InsertFarmerColl(FarmerCollectionInsertRequest request);
        Task UpdateFarmerColl(FarmerCollectionUpdateRequest request);
        Task DeleteFarmerColl(int id, int userId);
        Task<IEnumerable<CommonLists>> GetSpecificLists(FarmerCollectionRequest request);
        Task<IListsResponse<FarmerCollectionResponse>> GetAll(IListsRequest request);
        Task<FarmerCollectionResponse?> GetById(int id);
        Task<IEnumerable<FarmerCollectionResponse>> GetFarmerCollections(FarmerCollectionRequest request);
    }
}
