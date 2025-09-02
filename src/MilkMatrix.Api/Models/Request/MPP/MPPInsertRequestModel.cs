namespace MilkMatrix.Api.Models.Request.MPP
{
    public class MPPInsertRequestModel
    {
        public string Code { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string MPPName { get; set; } = string.Empty;
        public int BmcId { get; set; }
        public int RouteID { get; set; }
        public string? ShortName { get; set; }
        public string? RegionalName { get; set; }
        public string MPPExCode { get; set; } = string.Empty;
        public string? RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Logo { get; set; }
        public string? PunchLine { get; set; }
        public int? StateID { get; set; }
        public int? DistrictID { get; set; }
        public int? TehsilID { get; set; }
        public int? VillageID { get; set; }
        public int? HamletID { get; set; }

        public string? Address { get; set; }
        public string? RegionalAddress { get; set; }
        public string? Pincode { get; set; }
        public string? MobileNo { get; set; }
        public string? PhoneNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactRegionalName { get; set; }
        public string? Pancard { get; set; }

        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public string? AccNo { get; set; }
        public string? IFSC { get; set; }

        public int? NoOfVillageMapped { get; set; }
        public string? PouringMethod { get; set; }

        public bool IsActive { get; set; }

        public int? Business_entity_id { get; set; }
    }
}
