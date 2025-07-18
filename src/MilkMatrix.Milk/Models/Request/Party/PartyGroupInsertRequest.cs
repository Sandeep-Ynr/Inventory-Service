using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Party
{
    public class PartyGroupInsertRequest
    {
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string? GroupShortName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}

