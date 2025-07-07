using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;


namespace MilkMatrix.Milk.Contracts.Geographical
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
