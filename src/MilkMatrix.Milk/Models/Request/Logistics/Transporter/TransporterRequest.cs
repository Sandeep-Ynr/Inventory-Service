using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Logistics.Transporter
{
    public class TransporterRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string? TransporterID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
