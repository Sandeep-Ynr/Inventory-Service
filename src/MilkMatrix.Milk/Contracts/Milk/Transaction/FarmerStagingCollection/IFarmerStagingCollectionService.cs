using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerStagingCollection;

namespace MilkMatrix.Milk.Contracts.Milk.Transaction.FarmerStagingCollection
{
    public interface IFarmerStagingCollectionService
    {
        Task ImportFarmerCollection(FarmerCollStgInsertRequest request);
        //Task UpdateFarmerCollection(FarmerCollStgUpdateRequest request);
        //Task DeleteFarmerCollection(long id, int userId);
        Task<FarmerCollResponse?> GetFarmerCollectionExportById(long id);
        Task<IListsResponse<FarmerstagingCollResponse>> GetFarmerCollectionExport(IListsRequest request);
    }
}
