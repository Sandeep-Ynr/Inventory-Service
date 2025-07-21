using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Logistics.Vendor;
using MilkMatrix.Milk.Models.Response.Logistics.Vendor;

namespace MilkMatrix.Milk.Contracts.Logistics.Vendor
{
    public interface IVendorService
    {
        Task AddVendor(VendorInsertRequest request);
        Task UpdateVendor(VendorUpdateRequest request);
        Task DeleteVendor(int vendorId, long userId);
        Task<VendorResponse?> GetByVendorId(int vendorId);
        Task<IListsResponse<VendorResponse>> GetAll(IListsRequest request);
    }
}
