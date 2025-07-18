using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Party
{
    public class PartyUpdateRequest
    {
        public long PartyID { get; set; }
        public long GroupId { get; set; }
        public string PartyCode { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
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
        public bool IsActive { get; set; }
        public long? ModifyBy { get; set; }
    }
}
