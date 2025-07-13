using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Request.Plant;
using MilkMatrix.Milk.Models.Response.Geographical;
using MilkMatrix.Milk.Models.Response.Plant;

namespace MilkMatrix.Milk.Contracts.Plant
{
    public interface IPlantService
    {
        Task<IListsResponse<PlantResponse>> GetAllAsync(IListsRequest request);
        Task<PlantInsertResponse?> GetByIdAsync(int id);
        Task AddPlantAsync(PlantInsertRequest request);
        Task UpdatePlantAsync(PlantUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
