namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyUpdateRequestModel
    {
        public int BusinessId { get; set; }          // business_id
        public int PartyId { get; set; }             // party_id (PK, Identity)
        public long? GroupId { get; set; }           // group_id (nullable bigint)
        public string? PartyCode { get; set; }       // varchar(30)
        public string PartyName { get; set; } = "";  // varchar(200), NOT NULL
        public string? Gender { get; set; }          // varchar(10)
        public string? Mobile { get; set; }          // varchar(15)
        public string? Pan { get; set; }             // varchar(10)
        public string? Gstin { get; set; }           // varchar(15)
        public bool Is_Status { get; set; }           // bit
        public int modify_by { get; set; }           // bit


        public List<PartyBankAccountUdateRequestModel>? BankAccounts { get; set; }
        public List<PartyLocationUdateRequestModel>? Location { get; set; }
        public List<MemberProfileUdateRequestModel>? memberProfiles { get; set; }
        public List<PartyRoleUdateRequestModel>? Role { get; set; }
    }

    public class PartyBankAccountUdateRequestModel
    {
        public int BusinessId { get; set; }             // business_id
        public int Id { get; set; }                     // id (PK, Identity)
        public int PartyId { get; set; }                // party_id (FK → party)
        public string AccountHolder { get; set; } = ""; // account_holder (NOT NULL)
        public string AccountNumber { get; set; } = ""; // account_number (NOT NULL)
        public int BankId { get; set; }                 // bank_id (FK → tbl_BankMaster)
        public int BankBranchId { get; set; }           // bank_branch_id (FK → tbl_BranchMaster)
        public bool Is_Primary { get; set; }             // is_primary
        public bool Is_Status { get; set; }           // bit
        public int modify_by { get; set; }
    }

    public class PartyLocationUdateRequestModel
    {
        public int BusinessId { get; set; }             // business_id
        public int PartyLocationId { get; set; }        // party_location_id (PK, Identity)
        public int PartyId { get; set; }                // party_id (FK → tbl_party_new)
        public string? AddressType { get; set; }        // varchar(20)
        public string Line1 { get; set; } = "";         // varchar(150), NOT NULL
        public string? Line2 { get; set; }              // varchar(150)
        public string City { get; set; } = "";          // varchar(80), NOT NULL
        public int Stateid { get; set; }          // varchar(80), NOT NULL
        public string Pincode { get; set; } = "";       // varchar(10), NOT NULL
        public int Countryid { get; set; }  // varchar(60), default 'India'
        public string? ContactName { get; set; }        // varchar(150)
        public string? Mobile { get; set; }             // varchar(15)
        public string? Email { get; set; }              // varchar(120)
        public bool IsPrimary { get; set; }             // is_primary (default 0)
        public bool Is_Status { get; set; }           // bit
        public int modify_by { get; set; }

    }

    public class MemberProfileUdateRequestModel
    {
        public int BusinessId { get; set; }             // business_id (FK → tbl_business_details)
        public int PartyId { get; set; }                // party_id (PK, FK → party)
        public string MemberCode { get; set; } = "";    // member_code (NOT NULL)
        public int? MppId { get; set; }                 // mpp_id
        public int? SocietyId { get; set; }             // society_id
        public int? RouteId { get; set; }               // route_id
        public int? PreferredShiftId { get; set; }      // preferred_shift_id
        public DateTime? PouringStartDate { get; set; } // pouring_start_date
        public string? PaymentMode { get; set; }        // payment_mode
        public int? BankAccountId { get; set; }         // bank_account_id (FK → party_bank_account)
        public bool Is_Status { get; set; }           // bit
        public int modify_by { get; set; }

    }

    public class PartyRoleUdateRequestModel
    {
        public int BusinessId { get; set; }
        public int PartyRoleId { get; set; }   // PK
        public int PartyId { get; set; }       // FK → Party
        public int RoleStatusId { get; set; }  // FK → Status
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool Is_Status { get; set; }           // bit
        public int modify_by { get; set; }

    }
}
