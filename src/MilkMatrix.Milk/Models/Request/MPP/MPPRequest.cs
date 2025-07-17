using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.MPP
{
    public class MPPRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? MPPID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
