using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Party
{
    public class PartyGroupRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? GroupId { get; set; }
        public bool? IsStatus { get; set; }
    }
}
