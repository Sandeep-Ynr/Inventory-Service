using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;
using MilkMatrix.Milk.Models.Response.Accounts.AccountGroups;
namespace MilkMatrix.Milk.Contracts.Accounts.AccountGroups
{
    public interface IAccountGroupsService
    {
        Task InsertAccountGroup(AccountGroupsInsertRequest request);

        Task UpdateAccountGroup(int id, AccountGroupsUpdateRequest request);
        
        Task<AccountGroupsResponse?> GetAccountGroupById(int id);
        Task DeleteAccountGroupById(int id, int userId);
        //Task MoveAccountGroup(int id,int new_parent_id);

        Task<IListsResponse<AccountGroupsResponse>> GetAccountGroupList(IListsRequest request);


    }

    
}
