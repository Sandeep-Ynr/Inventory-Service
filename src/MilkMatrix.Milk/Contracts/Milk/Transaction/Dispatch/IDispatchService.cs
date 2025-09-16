using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Milk.Transactions.Dispatch;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.Dispatch;

namespace MilkMatrix.Milk.Contracts.Milk.Transaction.Dispatch
{
    public interface IDispatchService
    {
        Task AddDispatch(DispatchInsertRequest request);
        Task UpdateDispatch(DispatchUpdateRequest request);
        Task Delete(decimal rowId, int userId);
        Task<DispatchResponse?> GetById(decimal rowId);
        Task<IListsResponse<DispatchResponse>> GetAll(IListsRequest request);
    }
}
