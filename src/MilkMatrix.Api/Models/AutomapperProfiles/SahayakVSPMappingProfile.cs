using AutoMapper;
using MilkMatrix.Api.Models.Request.SahayakVSP;
using MilkMatrix.Milk.Models.Request.SahayakVSP;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class SahayakVSPMappingProfile : Profile
    {
        public SahayakVSPMappingProfile()
        {
            CreateMap<SahayakVSPInsertRequestModel, SahayakVSPInsertRequest>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.MPPCode, opt => opt.MapFrom(src => src.MPPCode))
                .ForMember(dest => dest.SahayakName, opt => opt.MapFrom(src => src.SahayakName))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.ExSahayakCode, opt => opt.MapFrom(src => src.ExSahayakCode))
                .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.PunchLine, opt => opt.MapFrom(src => src.PunchLine))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.RegionalAddress, opt => opt.MapFrom(src => src.RegionalAddress))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.ContactRegionalName, opt => opt.MapFrom(src => src.ContactRegionalName))
                .ForMember(dest => dest.Pancard, opt => opt.MapFrom(src => src.Pancard))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.AccNo, opt => opt.MapFrom(src => src.AccNo))
                .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<SahayakVSPUpdateRequestModel, SahayakVSPUpdateRequest>()
                .ForMember(dest => dest.SahayakID, opt => opt.MapFrom(src => src.SahayakID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.MPPCode, opt => opt.MapFrom(src => src.MPPCode))
                .ForMember(dest => dest.SahayakName, opt => opt.MapFrom(src => src.SahayakName))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.ExSahayakCode, opt => opt.MapFrom(src => src.ExSahayakCode))
                .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.RegistrationDate))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logo))
                .ForMember(dest => dest.PunchLine, opt => opt.MapFrom(src => src.PunchLine))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.RegionalAddress, opt => opt.MapFrom(src => src.RegionalAddress))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.ContactRegionalName, opt => opt.MapFrom(src => src.ContactRegionalName))
                .ForMember(dest => dest.Pancard, opt => opt.MapFrom(src => src.Pancard))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.AccNo, opt => opt.MapFrom(src => src.AccNo))
                .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
        }
    }
}
