using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Member
{
    public class MemberUpdateRequest
    {
        public int? MemberID { get; set; }
        public string? MemberCode { get; set; }
        public string? FarmerName { get; set; }
        public string? LocalName { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? AlternateNo { get; set; }
        public string? EmailID { get; set; }
        public string? AadharNo { get; set; }
        public long SocietyID { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
