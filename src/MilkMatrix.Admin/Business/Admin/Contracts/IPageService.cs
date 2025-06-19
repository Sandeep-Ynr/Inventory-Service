using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Page;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IPageService
{
    Task<Pages?> GetByIdAsync(int id);
    Task AddAsync(PageInsertRequest request);
    Task UpdateAsync(PageUpdateRequest request);
    Task DeleteAsync(int id, int userId);

    Task<IEnumerable<PageList>> GetAllAsync(Dictionary<string, object> filters);
}
