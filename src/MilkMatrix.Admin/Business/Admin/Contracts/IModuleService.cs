using MilkMatrix.Admin.Models.Admin.Requests.Module;
using MilkMatrix.Admin.Models.Admin.Responses.Modules;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IModuleService
{
    Task<ModuleDetails?> GetByIdAsync(int id);
    Task AddAsync(ModuleInsertRequest request);
    Task UpdateAsync(ModuleUpdateRequest request);
    Task DeleteAsync(int id, int userId);
    Task<IListsResponse<ModuleDetails>> GetAllAsync(IListsRequest request);
}
