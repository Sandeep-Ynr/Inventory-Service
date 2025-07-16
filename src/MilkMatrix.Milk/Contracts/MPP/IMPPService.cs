using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.MPP;

namespace MilkMatrix.Milk.Contracts.MPP
{
    public interface IMPPService
    {
        Task AddMPP(MPPInsertRequest request);
        Task UpdateMPP(MPPUpdateRequest request);
        Task Delete(int id, int userId);
        Task<MPPResponse?> GetById(int id);
        Task<IEnumerable<MPPResponse>> GetMPP(MPPRequest request);
        Task<IListsResponse<MPPResponse>> GetAll(IListsRequest request);
    }
}
