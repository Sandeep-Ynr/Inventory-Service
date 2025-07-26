using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? DocumentID { get; set; }
        public long? MemberID { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
