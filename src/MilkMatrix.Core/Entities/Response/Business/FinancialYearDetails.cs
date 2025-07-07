namespace MilkMatrix.Core.Entities.Response.Business;

public class FinancialYearDetails
{
    public int Id { get; set; }

    public string Fyear { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

}
