using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharCardBase64 { get; set; }
        public string? VoterIDBase64 { get; set; }
        public string? OtherDocumentBase64 { get; set; }
    }
}
