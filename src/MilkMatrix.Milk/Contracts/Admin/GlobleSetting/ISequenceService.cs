using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Response.MPP;

namespace MilkMatrix.Milk.Contracts.Admin.GlobleSetting
{
    public interface ISequenceService
    {
        Task Insertsequence(SequenceInsertRequest request);
        Task Updatesequence(SequenceUpdateRequest request);
        Task DeleteSequance(int id, int userId);
        Task<SequenceResponse?> GetSequanceById(string HeadName);
        Task<NextNumberResponse> GetNextNumberforSeq(string HeadName);
        Task<IListsResponse<SequenceResponse>>  GetSequanceList(IListsRequest request);
        
    }
}
