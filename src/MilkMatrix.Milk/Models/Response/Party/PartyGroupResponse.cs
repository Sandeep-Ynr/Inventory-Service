using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Party
{
    public class PartyGroupResponse : CommonLists
    {
        public string GroupCode { get; set; } = string.Empty;
        public string? GroupShortName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
    }
}
