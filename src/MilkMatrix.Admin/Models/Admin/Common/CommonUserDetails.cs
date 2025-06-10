using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Admin.Models.Admin.Common
{
    public class CommonUserDetails
    {
        public IEnumerable<Roles>? Roles { get; set; }

        public IEnumerable<ReportingDetails>? ReportingDetails { get; set; }

        public IEnumerable<CommonProps>? UserTypes { get; set; }

        public IEnumerable<BusinessData>? BusinessDetails { get; set; }

        public IEnumerable<SiteDetails>? SiteDetails { get; set; }
    }
}
