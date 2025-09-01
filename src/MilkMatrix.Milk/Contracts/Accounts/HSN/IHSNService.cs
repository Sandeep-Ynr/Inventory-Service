using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models.Request.Accounts.HSN;
using MilkMatrix.Milk.Models.Response.Accounts.HSN;
namespace MilkMatrix.Milk.Contracts.Accounts.HSN
{
    public interface IHSNService
    {
        Task InsertHSN(HSNInsertRequest request);

        Task UpdateHSN(int id, HSNUpdateRequest request);

        Task<HSNResponse?> GetHSNById(int id);
        Task DeleteHSNById(int id, int userId);
        //////Task MoveAccountGroup(int id,int new_parent_id);

        Task<IListsResponse<HSNResponse>> GetHSNList(IListsRequest request);


    }

    
}
