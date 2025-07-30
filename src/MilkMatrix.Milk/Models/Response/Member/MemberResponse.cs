using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
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
        public long? MemberID { get; set; }
        public string? AddressList { get; set; }
        public string? BankList { get; set; }
        public string? MilkProfileList { get; set; }
        public string? MilkDocumentList { get; set; }

    }
}
