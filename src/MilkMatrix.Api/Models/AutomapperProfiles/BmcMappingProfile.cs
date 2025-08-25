using AutoMapper;
using MilkMatrix.Api.Models.Request.Bmc;
using MilkMatrix.Api.Models.Request.Mcc;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Bmc;
using MilkMatrix.Milk.Models.Request.Mcc;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class BmcMappingProfile : Profile
    {
        public BmcMappingProfile() 
        {
            CreateMap<BmcUpdateRequestModel, BmcUpdateRequest>()
                        .ForMember(x => x.BmcId, opt => opt.MapFrom(src => src.BmcId))
                        .ForMember(x => x.BmcName, opt => opt.MapFrom(src => src.BmcName))
                        .ForMember(x => x.BmcCode, opt => opt.MapFrom(src => src.BmcCode))
                        .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                        .ForMember(x => x.MccId, opt => opt.MapFrom(src => src.MccId))
                        .ForMember(x => x.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                        .ForMember(x => x.Capacity, opt => opt.MapFrom(src => src.Capacity))
                        .ForMember(x => x.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer))
                        .ForMember(x => x.Model, opt => opt.MapFrom(src => src.Model))
                        .ForMember(x => x.InstallationDate, opt => opt.MapFrom(src => src.InstallationDate))
                        .ForMember(x => x.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                        .ForMember(x => x.Remarks, opt => opt.MapFrom(src => src.Remarks))
                        .ForMember(x => x.AnimalTypeId, opt => opt.MapFrom(src => src.AnimalTypeId))
                        .ForMember(x => x.FSSSINumber, opt => opt.MapFrom(src => src.FSSSINumber))
                        .ForMember(x => x.MappedMppId, opt => opt.MapFrom(src => src.MappedMppId))
                        .ForMember(x => x.HasExtraTank, opt => opt.MapFrom(src => src.HasExtraTank))
                        .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
                        .ForMember(x => x.HamletId, opt => opt.MapFrom(src => src.HamletId))
                        .ForMember(x => x.Pincode, opt => opt.MapFrom(src => src.Pincode))
                        .ForMember(x => x.Latitude, opt => opt.MapFrom(src => src.Latitude))
                        .ForMember(x => x.Longitude, opt => opt.MapFrom(src => src.Longitude))
                        .ForMember(x => x.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                        .ForMember(x => x.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                        .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.EmailId))
                        .ForMember(x => x.IsWorking, opt => opt.MapFrom(src => src.IsWorking))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<BmcInsertRequestModel, BmcInsertRequest>()
                        .ForMember(x => x.BmcName, opt => opt.MapFrom(src => src.BmcName))
                        .ForMember(x => x.BmcCode, opt => opt.MapFrom(src => src.BmcCode))
                        .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                        .ForMember(x => x.RegionalName, opt => opt.MapFrom(src => src.RegionalName))
                        .ForMember(x => x.Capacity, opt => opt.MapFrom(src => src.Capacity))
                        .ForMember(x => x.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer))
                        .ForMember(x => x.Model, opt => opt.MapFrom(src => src.Model))
                        .ForMember(x => x.InstallationDate, opt => opt.MapFrom(src => src.InstallationDate))
                        .ForMember(x => x.SerialNo, opt => opt.MapFrom(src => src.SerialNo))
                        .ForMember(x => x.Remarks, opt => opt.MapFrom(src => src.Remarks))
                        .ForMember(x => x.AnimalTypeId, opt => opt.MapFrom(src => src.AnimalTypeId))
                        .ForMember(x => x.FSSSINumber, opt => opt.MapFrom(src => src.FSSSINumber))
                        .ForMember(x => x.MappedMppId, opt => opt.MapFrom(src => src.MappedMppId))
                        .ForMember(x => x.HasExtraTank, opt => opt.MapFrom(src => src.HasExtraTank))
                        .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
                        .ForMember(x => x.HamletId, opt => opt.MapFrom(src => src.HamletId))
                        .ForMember(x => x.Pincode, opt => opt.MapFrom(src => src.Pincode))
                        .ForMember(x => x.Latitude, opt => opt.MapFrom(src => src.Latitude))
                        .ForMember(x => x.Longitude, opt => opt.MapFrom(src => src.Longitude))
                        .ForMember(x => x.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                        .ForMember(x => x.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                        .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.EmailId))
                        .ForMember(x => x.IsWorking, opt => opt.MapFrom(src => src.IsWorking))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
