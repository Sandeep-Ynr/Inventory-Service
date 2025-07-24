using AutoMapper;
using MilkMatrix.Api.Models.Request.Member;
using MilkMatrix.Milk.Models.Request.Member;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {
            CreateMap<MemberInsertRequestModel, MemberInsertRequest>()
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.AlternateNo, opt => opt.MapFrom(src => src.AlternateNo))
                .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.EmailID))
                .ForMember(dest => dest.AadharNo, opt => opt.MapFrom(src => src.AadharNo))
                .ForMember(dest => dest.SocietyID, opt => opt.MapFrom(src => src.SocietyID))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberUpdateRequestModel, MemberUpdateRequest>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.AlternateNo, opt => opt.MapFrom(src => src.AlternateNo))
                .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.EmailID))
                .ForMember(dest => dest.AadharNo, opt => opt.MapFrom(src => src.AadharNo)) 
                .ForMember(dest => dest.SocietyID, opt => opt.MapFrom(src => src.SocietyID))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
        }
    }
}
