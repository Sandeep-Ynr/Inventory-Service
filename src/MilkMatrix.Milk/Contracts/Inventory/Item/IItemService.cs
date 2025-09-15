using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Inventory.Item;

namespace MilkMatrix.Milk.Contracts.Inventory.Item
{
    public interface IItemService
    {
        Task AddItem(ItemInsertRequest request);
        Task UpdateItem(ItemUpdateRequest request);
        Task Delete(long id, int userId);
        Task<IListsResponse<ItemListResponse>> GetAll(IListsRequest request);
        Task<ItemResponse?> GetById(long id);
    }
}
