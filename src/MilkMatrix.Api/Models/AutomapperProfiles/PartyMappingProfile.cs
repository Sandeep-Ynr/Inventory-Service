using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Api.Models.Request.Party;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request.Approval.Level;
using MilkMatrix.Milk.Models.Request.Party;
using static Dapper.SqlMapper;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class PartyMappingProfile : Profile
    {
        public PartyMappingProfile()
        {
                   CreateMap<PartyGroupInsertRequestModel, PartyGroupInsertRequest>()
                  .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.GroupCode))
                  .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
                  .ForMember(dest => dest.GroupShortName, opt => opt.MapFrom(src => src.GroupShortName))
                  .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                  .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

                  CreateMap<PartyGroupUpdateRequestModel, PartyGroupUpdateRequest>()
                  .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
                  .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
                  .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                  .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));

                  CreateMap<PartyInsertRequestModel, PartyInsertRequest>()
                 .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
                 .ForMember(dest => dest.PartyCode, opt => opt.MapFrom(src => src.PartyCode))
                 .ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.PartyName))
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                 .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                 .ForMember(dest => dest.Pan, opt => opt.MapFrom(src => src.Pan))
                 .ForMember(dest => dest.Gstin, opt => opt.MapFrom(src => src.Gstin))
                 .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                 .ForMember(dest => dest.created_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["CreatedBy"]));

                 CreateMap<PartyUpdateRequestModel, PartyUpdateRequest>()
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.PartyCode, opt => opt.MapFrom(src => src.PartyCode))
                .ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.PartyName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.Pan, opt => opt.MapFrom(src => src.Pan))
                .ForMember(dest => dest.Gstin, opt => opt.MapFrom(src => src.Gstin))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.modify_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["ModifiedBy"]));

                 


                CreateMap<PartyRoleInsertRequestModel, PartyRoleInsertRequest>()
                .ForMember(dest => dest.RoleStatusId, opt => opt.MapFrom(src => src.RoleStatusId))
                .ForMember(dest => dest.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom))
                .ForMember(dest => dest.EffectiveTo, opt => opt.MapFrom(src => src.EffectiveTo))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.created_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["CreatedBy"]));

                CreateMap<PartyRoleUdateRequestModel, PartyRoleUdateRequest>()
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId))
                .ForMember(dest => dest.PartyRoleId, opt => opt.MapFrom(src => src.PartyRoleId))
                .ForMember(dest => dest.RoleStatusId, opt => opt.MapFrom(src => src.RoleStatusId)) // ✅ (you missed earlier, now added)
                .ForMember(dest => dest.EffectiveFrom, opt => opt.MapFrom(src => src.EffectiveFrom))
                .ForMember(dest => dest.EffectiveTo, opt => opt.MapFrom(src => src.EffectiveTo))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.modify_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["ModifiedBy"]));


            // ✅ New: PartyBankAccount mapping
            CreateMap<PartyBankAccountInsertRequestModel, PartyBankAccountInsertRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.AccountHolder, opt => opt.MapFrom(src => src.AccountHolder))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.BankId, opt => opt.MapFrom(src => src.BankId))
                .ForMember(dest => dest.BankBranchId, opt => opt.MapFrom(src => src.BankBranchId))
                .ForMember(dest => dest.Is_Primary, opt => opt.MapFrom(src => src.Is_Primary))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.created_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["CreatedBy"]));

            CreateMap<PartyBankAccountUdateRequestModel, PartyBankAccountUpdateRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId))
                .ForMember(dest => dest.AccountHolder, opt => opt.MapFrom(src => src.AccountHolder))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.BankId, opt => opt.MapFrom(src => src.BankId))
                .ForMember(dest => dest.BankBranchId, opt => opt.MapFrom(src => src.BankBranchId))
                .ForMember(dest => dest.Is_Primary, opt => opt.MapFrom(src => src.Is_Primary))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.modify_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["ModifiedBy"]));


            // ✅ PartyLocation Mapping
            CreateMap<PartyLocationInsertRequestModel, PartyLocationInsertRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType))
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Line1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.Line2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Stateid, opt => opt.MapFrom(src => src.Stateid))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.Countryid, opt => opt.MapFrom(src => src.Countryid))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Is_Primary, opt => opt.MapFrom(src => src.Is_Primary))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.created_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["CreatedBy"]));

            CreateMap<PartyLocationUdateRequestModel, PartyLocationUpdateRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.PartyLocationId, opt => opt.MapFrom(src => src.PartyLocationId))
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId))
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressType))
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Line1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.Line2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Stateid, opt => opt.MapFrom(src => src.Stateid))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.Countryid, opt => opt.MapFrom(src => src.Countryid))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Is_Primary, opt => opt.MapFrom(src => src.IsPrimary))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.modify_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["ModifiedBy"]));

            // ✅ MemberProfile Mapping
            CreateMap<MemberProfileInsertRequestModel, MemberProfileInsertRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.MppId, opt => opt.MapFrom(src => src.MppId))
                .ForMember(dest => dest.SocietyId, opt => opt.MapFrom(src => src.SocietyId))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                .ForMember(dest => dest.PreferredShiftId, opt => opt.MapFrom(src => src.PreferredShiftId))
                .ForMember(dest => dest.PouringStartDate, opt => opt.MapFrom(src => src.PouringStartDate))
                .ForMember(dest => dest.PaymentMode, opt => opt.MapFrom(src => src.PaymentMode))
                .ForMember(dest => dest.BankAccountId, opt => opt.MapFrom(src => src.BankAccountId))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.created_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["CreatedBy"]));

            CreateMap<MemberProfileUdateRequestModel, MemberProfileUpdateRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.PartyId, opt => opt.MapFrom(src => src.PartyId)) // PK reference, required for update
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.MppId, opt => opt.MapFrom(src => src.MppId))
                .ForMember(dest => dest.SocietyId, opt => opt.MapFrom(src => src.SocietyId))
                .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                .ForMember(dest => dest.PreferredShiftId, opt => opt.MapFrom(src => src.PreferredShiftId))
                .ForMember(dest => dest.PouringStartDate, opt => opt.MapFrom(src => src.PouringStartDate))
                .ForMember(dest => dest.PaymentMode, opt => opt.MapFrom(src => src.PaymentMode))
                .ForMember(dest => dest.BankAccountId, opt => opt.MapFrom(src => src.BankAccountId))
                .ForMember(dest => dest.Is_Status, opt => opt.MapFrom(src => src.Is_Status))
                .ForMember(dest => dest.modify_by, opt => opt.MapFrom((src, dest, m, context) => context.Items["ModifiedBy"]));








        }
    }
}
