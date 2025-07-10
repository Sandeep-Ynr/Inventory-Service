using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
namespace MilkMatrix.Milk.Contracts.Bank
{
    public interface IBankTypeService
    {
        Task<IEnumerable<BankTypeResponse>> GetBankTypes(BankTypeRequest request);
        Task<BankTypeResponse?> GetByBankTypeId(int id);
        Task AddBankType(BankTypeInsertRequest request);
        Task UpdateBankType(BankTypeUpdateRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(BankTypeRequest request);
        Task<IListsResponse<BankTypeResponse>> GetAll(IListsRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
