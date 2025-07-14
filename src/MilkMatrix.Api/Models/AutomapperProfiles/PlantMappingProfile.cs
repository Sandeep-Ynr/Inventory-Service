using AutoMapper;
using MilkMatrix.Api.Models.Request.Plant;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Plant;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class PlantMappingProfile : Profile
    {
        public PlantMappingProfile() 
        {
            CreateMap<PlantUpdateRequestModel, PlantUpdateRequest>()
                        .ForMember(x => x.PlantId, opt => opt.MapFrom(src => src.PlantId))
                        .ForMember(x => x.PlantName, opt => opt.MapFrom(src => src.PlantName))
                        .ForMember(x => x.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                        .ForMember(x => x.Capacity, opt => opt.MapFrom(src => src.Capacity))
                        .ForMember(x => x.FSSSINumber, opt => opt.MapFrom(src => src.FSSSINumber))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
                        .ForMember(x => x.HamletId, opt => opt.MapFrom(src => src.HamletId))
                        .ForMember(x => x.Pincode, opt => opt.MapFrom(src => src.Pincode))
                        .ForMember(x => x.Latitude, opt => opt.MapFrom(src => src.Latitude))
                        .ForMember(x => x.Longitude, opt => opt.MapFrom(src => src.Longitude))
                        .ForMember(x => x.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                        .ForMember(x => x.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                        .ForMember(x => x.RegionalContactPerson, opt => opt.MapFrom(src => src.RegionalContactPerson))
                        .ForMember(x => x.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                        .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.EmailId))
                        .ForMember(x => x.StartDate, opt => opt.MapFrom(src => src.StartDate))
                        .ForMember(x => x.IsWorking, opt => opt.MapFrom(src => src.IsWorking))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<PlantInsertRequestModel, PlantInsertRequest>()
                        .ForMember(x => x.PlantName, opt => opt.MapFrom(src => src.PlantName))
                        .ForMember(x => x.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                        .ForMember(x => x.Capacity, opt => opt.MapFrom(src => src.Capacity))
                        .ForMember(x => x.FSSSINumber, opt => opt.MapFrom(src => src.FSSSINumber))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
                        .ForMember(x => x.HamletId, opt => opt.MapFrom(src => src.HamletId))
                        .ForMember(x => x.Pincode, opt => opt.MapFrom(src => src.Pincode))
                        .ForMember(x => x.Latitude, opt => opt.MapFrom(src => src.Latitude))
                        .ForMember(x => x.Longitude, opt => opt.MapFrom(src => src.Longitude))
                        .ForMember(x => x.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                        .ForMember(x => x.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                        .ForMember(x => x.RegionalContactPerson, opt => opt.MapFrom(src => src.RegionalContactPerson))
                        .ForMember(x => x.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                        .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.EmailId))
                        .ForMember(x => x.StartDate, opt => opt.MapFrom(src => src.StartDate))
                        .ForMember(x => x.IsWorking, opt => opt.MapFrom(src => src.IsWorking))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
