using MilkMatrix.Admin.Models.Admin;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IPageService
{
    Task<Pages?> GetByIdAsync(int id);
    Task AddAsync(PageInsertRequest request);
    Task UpdateAsync(PageUpdateRequest request);
    Task DeleteAsync(int id, int userId);

    Task<IListsResponse<Pages>> GetAllAsync(IListsRequest request);

    /// <summary>
    /// Retrieves the details of a actions.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<CommonLists>?> GetPagesForApprovalAsync(int? id = null);
}
