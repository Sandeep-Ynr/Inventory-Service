using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.MPP
{
    public class MPPResponse : CommonLists
    {
        public int MPPID { get; set; }

        public string? Code { get; set; }
        public string? CompanyCode { get; set; }
        public string? MPPName { get; set; }
        public string? ShortName { get; set; }
        public string? RegionalName { get; set; }
        public string? MPPExCode { get; set; }
        public string? RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Logo { get; set; }
        public string? PunchLine { get; set; }

        public int StateID { get; set; }
        public string? StateName { get; set; }

        public int DistrictID { get; set; }
        public string? DistrictName { get; set; }

        public int TehsilID { get; set; }
        public string? TehsilName { get; set; }

        public int VillageID { get; set; }
        public string? VillageName { get; set; }

        public int HamletID { get; set; }
        public string? HamletName { get; set; }

        public string? Address { get; set; }
        public string? RegionalAddress { get; set; }
        public string? Pincode { get; set; }
        public string? MobileNo { get; set; }
        public string? PhoneNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactRegionalName { get; set; }
        public string? Pancard { get; set; }

        public int BankID { get; set; }
        public string? BankName { get; set; }

        public int BranchID { get; set; }
        public string? BranchName { get; set; }

        public string? AccNo { get; set; }
        public string? IFSC { get; set; }

        public int? NoOfVillageMapped { get; set; }
        public string? PouringMethod { get; set; }

        public bool? IsActive { get; set; } // Corresponds to IsStatus
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
