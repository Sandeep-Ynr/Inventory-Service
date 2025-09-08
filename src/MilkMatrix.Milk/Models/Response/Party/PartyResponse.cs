using MilkMatrix.Milk.Models.Request.Party;

public class PartyResponse
{
    public int BusinessId { get; set; }
    public long PartyId { get; set; }
    public int GroupId { get; set; }
    public string?    PartyCode { get; set; }
    public string? PartyName { get; set; }
    public string? Gender { get; set; }
    public string? Mobile { get; set; }
    public string? Pan { get; set; }
    public string? Gstin { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    //public List<PartyBankAccount>? BankAccounts { get; set; }
    //public List<PartyLocation>? Location { get; set; }
    //public List<MemberProfile>? memberProfiles { get; set; }
    //public List<PartyRole>? Role { get; set; }



}

public class PartyDetailResponse
{
    public int BusinessId { get; set; }
    public long PartyId { get; set; }
    public int GroupId { get; set; }
    public string? PartyCode { get; set; }
    public string? PartyName { get; set; }
    public string? Gender { get; set; }
    public string? Mobile { get; set; }
    public string? Pan { get; set; }
    public string? Gstin { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public List<PartyBankAccount>? BankAccounts { get; set; }
    public List<PartyLocation>? Location { get; set; }
    public List<MemberProfiles>? MemberProfiles { get; set; }
    public List<PartyRoles>? Role { get; set; }



}

public class PartyDetailResponseraw
{
    public int BusinessId { get; set; }
    public long PartyId { get; set; }
    public int GroupId { get; set; }
    public string? PartyCode { get; set; }
    public string? PartyName { get; set; }
    public string? Gender { get; set; }
    public string? Mobile { get; set; }
    public string? Pan { get; set; }
    public string? Gstin { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }

    public string? BankAccounts { get; set; }
    public string? Locations { get; set; }
    public string? MemberProfiles { get; set; }
    public string? Roles { get; set; }
}


public class PartyBankAccount
{
    public int BusinessId { get; set; }
    public int Id { get; set; }
    public long PartyId { get; set; }
    public string? AccountHolder { get; set; }
    public string? AccountNumber { get; set; }
    public int BankId { get; set; }
    public int BankBranchId { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
}

public class PartyLocation
{
    public int BusinessId { get; set; }
    public int PartyLocationId { get; set; }
    public long PartyId { get; set; }
    public string? AddressType { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? City { get; set; }
    public int StateId { get; set; }
    public string? Pincode { get; set; }
    public int CountryId { get; set; }
    public string? ContactName { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
}

public class MemberProfiles
{
    public int BusinessId { get; set; }
    public long PartyId { get; set; }
    public string? MemberCode { get; set; }
    public int? MppId { get; set; }
    public int? SocietyId { get; set; }
    public int? RouteId { get; set; }
    public int? PreferredShiftId { get; set; }
    public DateTime? PouringStartDate { get; set; }
    public string? PaymentMode { get; set; }
    public int? BankAccountId { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
}

public class PartyRoles
{
    public int BusinessId { get; set; }
    public int PartyRoleId { get; set; }
    public long PartyId { get; set; }
    public int RoleStatusId { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
}
