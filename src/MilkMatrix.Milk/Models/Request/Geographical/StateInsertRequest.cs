using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class StateInsertRequest
    {
        public string StateName { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public string? AreaCode { get; set; }
        public CrudActionType ActionType { get; set; }

        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
