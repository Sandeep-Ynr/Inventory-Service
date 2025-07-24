using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member
{
    public class MemberResponse : CommonLists
    {
        public string? MemberCode { get; set; }
        public string? LocalName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? AlternateNo { get; set; }
        public string? EmailID { get; set; }
        public string? AadharNo { get; set; }
        public long SocietyID { get; set; }
        public string? SocietyName { get; set; } 
    }
}
