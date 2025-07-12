using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Geographical;
namespace MilkMatrix.Milk.Contracts.Bank
{
    public interface IBankService
    {
        Task<IListsResponse<BankResponse>> GetAll(IListsRequest request);
        Task<IEnumerable<BankResponse>> GetBank(BankRequest request);
        Task<BankResponse?> GetById(int id);
        Task AddBank(BankInsertRequest request);
        Task UpdateBank(BankUpdateRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(BankRequest request);
        Task Delete(int id, int userId);
    }
}
