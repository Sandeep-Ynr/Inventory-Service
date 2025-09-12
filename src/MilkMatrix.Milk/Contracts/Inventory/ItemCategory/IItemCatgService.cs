using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Response.Inventory.ItemCategory;

namespace MilkMatrix.Milk.Contracts.Inventory.ItemCategory
{
    public interface IItemCatgService
    {
        Task AddItemCatg(ItemCatgInsertRequest request);
        Task UpdateItemCatg(ItemCatgUpdateRequest request);
        Task Delete(int id, int userId);
        Task<ItemCatgResponse?> GetById(int id);
        Task<IEnumerable<ItemCatgResponse>> GetItemCatg(ItemCatgRequest request);
        Task<IListsResponse<ItemCatgResponse>> GetAll(IListsRequest request);
    }
}
