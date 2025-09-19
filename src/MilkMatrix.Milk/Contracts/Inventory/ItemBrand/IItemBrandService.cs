using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Inventory.Item;
using MilkMatrix.Milk.Models.Request.Inventory.ItemBrand;
using MilkMatrix.Milk.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Response.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Inventory.ItemBrand;
using MilkMatrix.Milk.Models.Response.Inventory.ItemCategory;

namespace MilkMatrix.Milk.Contracts.Inventory.Item
{
    public interface IItemBrandService
    {
        Task AddItemBrand(ItemBrandInsertRequest request);
        Task UpdateItemBrand(ItemBrandUpdateRequest  request);
        Task Delete(int id, int userId);
        Task<ItemBrandResponse ?> GetById(int id);
        //Task<IEnumerable<ItemBrandResponse>> GetItemCatg(ItemBrandRequest request);
        Task<IListsResponse<ItemBrandResponse>> GetAll(IListsRequest request);
    }
}
