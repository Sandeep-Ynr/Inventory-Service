using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Route
{
    public class RouteRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? RouteID { get; set; }
        public bool? IsActive { get; set; }
    }
}
