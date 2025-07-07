using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Core.Entities.Response.Business;

namespace MilkMatrix.Admin.Models.Admin.Common;

public class CommonUserDetails
{
    public IEnumerable<Roles>? Roles { get; set; }

    public IEnumerable<ReportingDetails>? ReportingDetails { get; set; }

    public IEnumerable<CommonProps>? UserTypes { get; set; }

    public IEnumerable<BusinessData>? BusinessDetails { get; set; }

    public IEnumerable<SiteDetails>? SiteDetails { get; set; }

    public IEnumerable<FinancialYearDetails>? FinancialYearDetails { get; set; }
}
