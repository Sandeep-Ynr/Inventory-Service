using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.Logistics.Transporter;
using MilkMatrix.Milk.Models.Response.MPP;

namespace MilkMatrix.Milk.Contracts.Logistics.Transporter
{
    public interface ITransporterService
    {
        Task AddTransporter(TransporterInsertRequest request);
        Task UpdateTransporter(TransporterUpdateRequest request);
        Task Delete(string transporterId, int userId);
        Task<TransporterResponse?> GetById(int id);
        Task<IEnumerable<TransporterResponse>> GetTransporters(TransporterRequest request);
        Task<IListsResponse<TransporterResponse>> GetAll(IListsRequest request);
    }
}
