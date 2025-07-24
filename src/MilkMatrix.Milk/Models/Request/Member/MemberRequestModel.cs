using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member
{
    public class MemberRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string? MemberID { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public long? SocietyID { get; set; }
        public string? MobileNo { get; set; }
        public string? AadharNo { get; set; }
    }
}
