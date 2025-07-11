using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;

namespace MilkMatrix.Milk.Contracts.Bank
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchResponse>> GetBranches(BranchRequest request);
        Task<BranchResponse?> GetByBranchId(int id);
        Task<IListsResponse<BranchResponse>> GetAll(IListsRequest request);
        Task AddBranch(BranchInsertRequest request);
        Task UpdateBranch(BranchUpdateRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(BranchRequest request);
        Task Delete(int id, int userId);
    }
}
