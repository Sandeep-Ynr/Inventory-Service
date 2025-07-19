using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Logistics.Vehicle
{
    public class VehicleRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? VehicleId { get; set; }
        public string? RegistrationNo { get; set; }
        public string? TransporterCode { get; set; }
        public bool? IsActive { get; set; }
    }
}
