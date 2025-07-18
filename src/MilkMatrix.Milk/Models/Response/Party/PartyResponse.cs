using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Party
{
    public class PartyResponse : CommonLists
    {
        public string PartyCode { get; set; } = string.Empty;
        public string? PartyEmail { get; set; }
        public string? PartyShortName { get; set; }
        public string? PartyAddress { get; set; }
        public string? PartyPinCode { get; set; }
        public string? PartyPhoneNo { get; set; }
        public string? PartyLicenceNo { get; set; }
        public string? PartyGstNo { get; set; }
        public string? PartyOwnerName { get; set; }
        public string? PartyOwnerEmail { get; set; }
        public string? PartyOwnerPhoneNo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
        public bool is_status { get; set; }
    }
}
