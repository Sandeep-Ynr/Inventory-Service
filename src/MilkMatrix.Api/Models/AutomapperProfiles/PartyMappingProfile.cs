using AutoMapper;
using Azure.Core;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Api.Models.Request.Party;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Milk.Models.Request.Party;

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
                .ForMember(dest => dest.PartyCode, opt => opt.MapFrom(src => src.PartyCode))
                .ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.PartyName))
                .ForMember(dest => dest.PartyEmail, opt => opt.MapFrom(src => src.PartyEmail))
                .ForMember(dest => dest.PartyShortName, opt => opt.MapFrom(src => src.PartyShortName))
                .ForMember(dest => dest.PartyAddress, opt => opt.MapFrom(src => src.PartyAddress))
                .ForMember(dest => dest.PartyPinCode, opt => opt.MapFrom(src => src.PartyPinCode))
                .ForMember(dest => dest.PartyPhoneNo, opt => opt.MapFrom(src => src.PartyPhoneNo))
                .ForMember(dest => dest.PartyLicenceNo, opt => opt.MapFrom(src => src.PartyLicenceNo))
                .ForMember(dest => dest.PartyGstNo, opt => opt.MapFrom(src => src.PartyGstNo))
                .ForMember(dest => dest.PartyOwnerName, opt => opt.MapFrom(src => src.PartyOwnerName))
                .ForMember(dest => dest.PartyOwnerEmail, opt => opt.MapFrom(src => src.PartyOwnerEmail))
                .ForMember(dest => dest.PartyOwnerPhoneNo, opt => opt.MapFrom(src => src.PartyOwnerPhoneNo))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));

            CreateMap<PartyUpdateRequestModel, PartyUpdateRequest>()
                .ForMember(dest => dest.PartyID, opt => opt.MapFrom(src => src.PartyID))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.PartyCode, opt => opt.MapFrom(src => src.PartyCode))
                .ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.PartyName))
                .ForMember(dest => dest.PartyEmail, opt => opt.MapFrom(src => src.PartyEmail))
                .ForMember(dest => dest.PartyShortName, opt => opt.MapFrom(src => src.PartyShortName))
                .ForMember(dest => dest.PartyAddress, opt => opt.MapFrom(src => src.PartyAddress))
                .ForMember(dest => dest.PartyPinCode, opt => opt.MapFrom(src => src.PartyPinCode))
                .ForMember(dest => dest.PartyPhoneNo, opt => opt.MapFrom(src => src.PartyPhoneNo))
                .ForMember(dest => dest.PartyLicenceNo, opt => opt.MapFrom(src => src.PartyLicenceNo))
                .ForMember(dest => dest.PartyGstNo, opt => opt.MapFrom(src => src.PartyGstNo))
                .ForMember(dest => dest.PartyOwnerName, opt => opt.MapFrom(src => src.PartyOwnerName))
                .ForMember(dest => dest.PartyOwnerEmail, opt => opt.MapFrom(src => src.PartyOwnerEmail))
                .ForMember(dest => dest.PartyOwnerPhoneNo, opt => opt.MapFrom(src => src.PartyOwnerPhoneNo))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));


        }
    }
}
