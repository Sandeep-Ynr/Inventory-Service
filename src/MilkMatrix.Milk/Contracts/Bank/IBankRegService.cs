using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;


namespace MilkMatrix.Milk.Contracts.Bank
{
    public interface IBankRegService
    {
        Task<BankRegResponse?> GetById(int BankRegId);
        Task<IEnumerable<BankRegResponse>> GetBankReg(BankRegionalRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(BankRegionalRequest request);
        Task AddBankReg(BankRegInsertRequest request);
        Task UpdateBankReg(BankRegUpdateRequest request);
        Task DeleteAsync(int id, int userId);
    }
}
