using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Bmc
{
    public class BmcIndividualResponse:CommonLists
    {
        //public int Id { get; set; }
        //public string? Name { get; set; }
        public int? BusinessEntityId { get; set; }
        public string? RegionalName { get; set; }
        public string? Capacity { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public DateTime? InstallationDate { get; set; }
        public string? SerialNo { get; set; }
        public string? Remarks { get; set; }
        public int? AnimalTypeId { get; set; }
        public string? FSSSINumber { get; set; }
        public int? MappedMppId { get; set; }
        public bool? HasExtraTank { get; set; }
        public string? Address { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TehsilId { get; set; }
        public int VillageId { get; set; }
        public int HamletId { get; set; }
        public int Pincode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public bool? IsWorking { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
    }
}
