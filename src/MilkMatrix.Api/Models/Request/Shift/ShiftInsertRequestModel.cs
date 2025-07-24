namespace MilkMatrix.Api.Models.Request.Shift
{
    public class ShiftInsertRequestModel
    {
        public string? ShiftCode { get; set; }
        public string? ShiftName { get; set; }
        public string? ShiftTime { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
