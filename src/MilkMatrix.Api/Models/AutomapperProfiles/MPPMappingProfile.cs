using AutoMapper;
using MilkMatrix.Api.Models.Request.MPP;
using MilkMatrix.Milk.Models.Request.MPP;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MPPMappingProfile : Profile
    {
        public MPPMappingProfile()
        {
            CreateMap<MPPInsertRequestModel, MPPInsertRequest>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.MPPName, opt => opt.MapFrom(src => src.MPPName))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.MPPExCode, opt => opt.MapFrom(src => src.MPPExCode))
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
                .ForMember(dest => dest.NoOfVillageMapped, opt => opt.MapFrom(src => src.NoOfVillageMapped))
                .ForMember(dest => dest.PouringMethod, opt => opt.MapFrom(src => src.PouringMethod))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Business_entity_id, opt => opt.MapFrom(src => src.Business_entity_id))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));


            CreateMap<MPPUpdateRequestModel, MPPUpdateRequest>()
                .ForMember(dest => dest.MPPID, opt => opt.MapFrom(src => src.MPPID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.MPPName, opt => opt.MapFrom(src => src.MPPName))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.MPPExCode, opt => opt.MapFrom(src => src.MPPExCode))
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
                .ForMember(dest => dest.NoOfVillageMapped, opt => opt.MapFrom(src => src.NoOfVillageMapped))
                .ForMember(dest => dest.PouringMethod, opt => opt.MapFrom(src => src.PouringMethod))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.Business_entity_id, opt => opt.MapFrom(src => src.Business_entity_id))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
        }
    }
}
