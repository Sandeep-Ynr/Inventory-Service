using MilkMatrix.Admin.Models.Admin.Requests.SubModule;
using MilkMatrix.Admin.Models.Admin.Responses.SubModules;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface ISubModuleService
{
    Task<SubModuleDetails?> GetByIdAsync(int id);
    Task AddAsync(SubModuleInsertRequest request);
    Task UpdateAsync(SubModuleUpdateRequest request);
    Task DeleteAsync(int id, int userId);
    Task<IListsResponse<SubModuleDetails>> GetAllAsync(IListsRequest request);
}
