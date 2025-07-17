using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Animal;
using MilkMatrix.Milk.Models.Response.Animal;

namespace MilkMatrix.Milk.Contracts.Animal
{
    public interface IAnimalService
    {
        Task<IListsResponse<AnimalTypeInsertResponse>> GetAllAsync(IListsRequest request);
        Task<AnimalTypeInsertResponse?> GetByIdAsync(int id);
        Task AddAsync(AnimalTypeInsertRequest request);
        Task UpdateAsync(AnimalTypeUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
