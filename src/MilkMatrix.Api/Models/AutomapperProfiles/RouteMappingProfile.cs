using AutoMapper;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Api.Models.Request.Route;
using MilkMatrix.Milk.Models.Request.Route;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class RouteMappingProfile : Profile
    {
        public RouteMappingProfile()
        {
            CreateMap<RouteInsertRequestModel, RouteInsertRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RouteCode, opt => opt.MapFrom(src => src.RouteCode))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.VehicleID, opt => opt.MapFrom(src => src.VehicleID))
                .ForMember(dest => dest.VehicleCapacity, opt => opt.MapFrom(src => src.VehicleCapacity))
                .ForMember(dest => dest.MorningStartTime, opt => opt.MapFrom(src => src.MorningStartTime))
                .ForMember(dest => dest.MorningEndTime, opt => opt.MapFrom(src => src.MorningEndTime))
                .ForMember(dest => dest.EveningStartTime, opt => opt.MapFrom(src => src.EveningStartTime))
                .ForMember(dest => dest.EveningEndTime, opt => opt.MapFrom(src => src.EveningEndTime))
                .ForMember(dest => dest.TotalKm, opt => opt.MapFrom(src => src.TotalKm))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));

            CreateMap<RouteUpdateRequestModel, RouteUpdateRequest>()
                .ForMember(dest => dest.RouteID, opt => opt.MapFrom(src => src.RouteID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RouteCode, opt => opt.MapFrom(src => src.RouteCode))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                .ForMember(dest => dest.VehicleID, opt => opt.MapFrom(src => src.VehicleID))
                .ForMember(dest => dest.VehicleCapacity, opt => opt.MapFrom(src => src.VehicleCapacity))
                .ForMember(dest => dest.MorningStartTime, opt => opt.MapFrom(src => src.MorningStartTime))
                .ForMember(dest => dest.MorningEndTime, opt => opt.MapFrom(src => src.MorningEndTime))
                .ForMember(dest => dest.EveningStartTime, opt => opt.MapFrom(src => src.EveningStartTime))
                .ForMember(dest => dest.EveningEndTime, opt => opt.MapFrom(src => src.EveningEndTime))
                .ForMember(dest => dest.TotalKm, opt => opt.MapFrom(src => src.TotalKm))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));
            CreateMap<VehicleTypeInsertRequestModel, VehicleTypeInsertRequest>()
                 .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
                 .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                 .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));
            CreateMap<VehicleTypeUpdateRequestModel, VehicleTypeUpdateRequest>()
                .ForMember(dest => dest.VehicleID, opt => opt.MapFrom(src => src.VehicleID))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
        }
    }
}
