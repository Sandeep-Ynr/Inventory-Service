using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Request.SahayakVSP;
using MilkMatrix.Milk.Models.Response.SahayakVSP;

namespace MilkMatrix.Milk.Contracts.SahayakVSP
{
    public interface ISahayakVSPService
    {
        Task AddSahayakVSP(SahayakVSPInsertRequest request);
        Task UpdateSahayakVSP(SahayakVSPUpdateRequest request);
        Task Delete(int id, int userId);
        Task<SahayakVSPResponse?> GetById(int id);
        Task<IEnumerable<SahayakVSPResponse>> GetSahayakVSPs(SahayakVSPRequest request);
        Task<IEnumerable<CommonLists>> GetSpecificLists(SahayakVSPRequest request);
        Task<IListsResponse<SahayakVSPResponse>> GetAll(IListsRequest request);
    }
}
