using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Admin.Common
{
    public class LoggedInUser
    {
        public int UserId { get; set; }
        public string? EmailId { get; set; }
        public string? MobileNo { get; set; }
        public string? BusinessId { get; set; }
        public int ReportingId { get; set; }
        public string? RoleId { get; set; }
        public string? UserName { get; set; }
        public int UserType { get; set; }
    }
}
