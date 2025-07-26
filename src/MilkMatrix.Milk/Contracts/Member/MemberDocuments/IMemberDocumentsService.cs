using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Milk.Models.Request.Member.MemberDocuments;
using MilkMatrix.Milk.Models.Response.Member.MemberDocuments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Contracts.Member.MemberDocuments
{
    public interface IMemberDocumentsService
    {
        Task AddMemberDocuments(MemberDocumentsInsertRequest request);
        Task UpdateMemberDocuments(MemberDocumentsUpdateRequest request);
        Task DeleteMemberDocuments(long documentId, long userId); // Assuming userId for tracking who deleted
        Task<MemberDocumentsResponse?> GetMemberDocumentsById(long id);
        Task<IEnumerable<MemberDocumentsResponse>> GetMemberDocumentsByMemberId(long memberId);
        Task<IListsResponse<MemberDocumentsResponse>> GetAllMemberDocuments(IListsRequest request);
    }
}