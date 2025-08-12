using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Plant
{
    public class PlantUpdateRequest
    {
        public int PlantId { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public int CompanyId { get; set; }
        public string? Capacity { get; set; }
        public string? FSSSINumber { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TehsilId { get; set; }
        public int VillageId { get; set; }
        public int HamletId { get; set; }
        public int Pincode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? RegionalName { get; set; }
        public string? ContactPerson { get; set; }
        public string? RegionalContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public DateOnly? StartDate { get; set; }
        public bool? IsWorking { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
