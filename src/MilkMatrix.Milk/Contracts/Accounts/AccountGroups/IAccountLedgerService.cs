using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;
using MilkMatrix.Milk.Models.Response.Accounts.AccountGroups;
namespace MilkMatrix.Milk.Contracts.Accounts.AccountGroups
{
    public interface IAccountLedgerService
    {
        Task InsertAccountLedger(AccountHeadsInsertRequest request);

        Task UpdateAccountLedger(int id, AccountHeadsUpdateRequest request);

        Task<AccountHeadsResponse?> GetAccountHeadById(int id);
        Task DeleteAccountHeadById(int id, int userId);
        ////Task MoveAccountGroup(int id,int new_parent_id);

        Task<IListsResponse<AccountHeadsResponse>> GetAccountHeadList(IListsRequest request);


    }

    
}
