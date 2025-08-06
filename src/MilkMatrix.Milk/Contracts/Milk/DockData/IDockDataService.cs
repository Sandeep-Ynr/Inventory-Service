using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Milk.DockData;
using MilkMatrix.Milk.Models.Response.Milk.DockData;

namespace MilkMatrix.Milk.Contracts.Milk.DockData
{
    public interface IDockDataService
    {
        Task InsertDockData(DockDataInsertRequest request);
        Task UpdateDockData(DockDataUpdateRequest request);
        Task DockDataDelete(int id, int userId);
        Task<DockDataResponse?> GetById(int id);
        Task<IEnumerable<DockDataResponse>> GetDockData(DockDataRequest request);
        Task<IListsResponse<DockDataResponse>> GetAll(IListsRequest request);
    }


}
