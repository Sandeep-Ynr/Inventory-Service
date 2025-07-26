using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MilkMatrix.Milk.Models.Request.Price
{
    public class BulkMilkPriceUploadRequest
    {
        //public string UserName { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        //public string EmailId { get; set; } = string.Empty;

        //public string HrmsCode { get; set; } = string.Empty;

        //public string RoleId { get; set; } = string.Empty;

        //public string BusinessId { get; set; } = string.Empty;

        //public string ReportingId { get; set; } = string.Empty;

        //public int? UserType { get; set; }

        //public string MobileNumber { get; set; } = string.Empty;

        //public int CreatedBy { get; set; }

        //public string? IsMFA { get; set; } = "N";

        //public string? IsBulkUser { get; set; } = "Y";

        //public string? ChangePassword { get; set; } = "Y";

        //public bool IsActive { get; set; } = true;

        //public string? ProcessStatus { get; set; } = "Pending";

        //public string? ErrorMessage { get; set; }

        public int? business_entity_id { get; set; }
        public DateTime? with_effect_date { get; set; }
        public string? rate_gen_type { get; set; }
        public string? description { get; set; }
        public int? milk_type_id { get; set; }
        public int? rate_type_id { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }

        public IFormFile CsvFile { get; set; }
    }
}
