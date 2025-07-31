using AutoMapper;
using Azure.Core;
using MilkMatrix.Api.Models.Request.Logistics.Route;
using MilkMatrix.Api.Models.Request.Logistics.Transporter;
using MilkMatrix.Api.Models.Request.Logistics.Vehicle;
using MilkMatrix.Api.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Api.Models.Request.Logistics.Vendor;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Milk.Models.Request.Logistics.Route;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Request.Logistics.VehcileType;
using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Request.Logistics.Vendor;
namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class LogisticsMappingProfile : Profile
    {
        public LogisticsMappingProfile()
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


            CreateMap<VehicleInsertRequestModel, VehicleInsertRequest>()
             .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleTypeId))
             .ForMember(dest => dest.CapacityCode, opt => opt.MapFrom(src => src.CapacityCode))
             .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
             .ForMember(dest => dest.ApplicableRTO, opt => opt.MapFrom(src => src.ApplicableRTO))
             .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.DriverName))
             .ForMember(dest => dest.DriverContactNo, opt => opt.MapFrom(src => src.DriverContactNo))
             .ForMember(dest => dest.WEFDate, opt => opt.MapFrom(src => src.WEFDate))
             .ForMember(dest => dest.DrivingLicenseNumber, opt => opt.MapFrom(src => src.DrivingLicenseNumber))
             .ForMember(dest => dest.LicenceExpiryDate, opt => opt.MapFrom(src => src.LicenceExpiryDate))
             .ForMember(dest => dest.TransporterCode, opt => opt.MapFrom(src => src.TransporterCode))
             .ForMember(dest => dest.MappedRoute, opt => opt.MapFrom(src => src.MappedRoute))
             .ForMember(dest => dest.PollutionCertificate, opt => opt.MapFrom(src => src.PollutionCertificate))
             .ForMember(dest => dest.Insurance, opt => opt.MapFrom(src => src.Insurance))
             .ForMember(dest => dest.RCBookNo, opt => opt.MapFrom(src => src.RCBookNo))
             .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
             .ForMember(dest => dest.Rent, opt => opt.MapFrom(src => src.Rent))
             .ForMember(dest => dest.Average, opt => opt.MapFrom(src => src.Average))
             .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
             .ForMember(dest => dest.FuelTypeCode, opt => opt.MapFrom(src => src.FuelTypeCode))
             .ForMember(dest => dest.PassingNo, opt => opt.MapFrom(src => src.PassingNo))
             .ForMember(dest => dest.BMCCode, opt => opt.MapFrom(src => src.BMCCode))
             .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
             .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));


            CreateMap<VehicleUpdateRequestModel, VehicleUpdateRequest>()
             .ForMember(dest => dest.VehicleTypeId, opt => opt.MapFrom(src => src.VehicleTypeId))
             .ForMember(dest => dest.CapacityCode, opt => opt.MapFrom(src => src.CapacityCode))
             .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
             .ForMember(dest => dest.ApplicableRTO, opt => opt.MapFrom(src => src.ApplicableRTO))
             .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.DriverName))
             .ForMember(dest => dest.DriverContactNo, opt => opt.MapFrom(src => src.DriverContactNo))
             .ForMember(dest => dest.WEFDate, opt => opt.MapFrom(src => src.WEFDate))
             .ForMember(dest => dest.DrivingLicenseNumber, opt => opt.MapFrom(src => src.DrivingLicenseNumber))
             .ForMember(dest => dest.LicenceExpiryDate, opt => opt.MapFrom(src => src.LicenceExpiryDate))
             .ForMember(dest => dest.TransporterCode, opt => opt.MapFrom(src => src.TransporterCode))
             .ForMember(dest => dest.MappedRoute, opt => opt.MapFrom(src => src.MappedRoute))
             .ForMember(dest => dest.PollutionCertificate, opt => opt.MapFrom(src => src.PollutionCertificate))
             .ForMember(dest => dest.Insurance, opt => opt.MapFrom(src => src.Insurance))
             .ForMember(dest => dest.RCBookNo, opt => opt.MapFrom(src => src.RCBookNo))
             .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
             .ForMember(dest => dest.Rent, opt => opt.MapFrom(src => src.Rent))
             .ForMember(dest => dest.Average, opt => opt.MapFrom(src => src.Average))
             .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
             .ForMember(dest => dest.FuelTypeCode, opt => opt.MapFrom(src => src.FuelTypeCode))
             .ForMember(dest => dest.PassingNo, opt => opt.MapFrom(src => src.PassingNo))
             .ForMember(dest => dest.BMCCode, opt => opt.MapFrom(src => src.BMCCode))
             .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
             .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));


            CreateMap<TransporterInsertRequestModel, TransporterInsertRequest>()
                .ForMember(dest => dest.TransporterName, opt => opt.MapFrom(src => src.TransporterName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.LocalContactPerson, opt => opt.MapFrom(src => src.LocalContactPerson))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
                .ForMember(dest => dest.BankAccountNo, opt => opt.MapFrom(src => src.BankAccountNo))
                .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
                .ForMember(dest => dest.GSTIN, opt => opt.MapFrom(src => src.GSTIN))
                .ForMember(dest => dest.TdsPer, opt => opt.MapFrom(src => src.TdsPer))
                .ForMember(dest => dest.PanNo, opt => opt.MapFrom(src => src.PanNo))
                .ForMember(dest => dest.BeneficiaryName, opt => opt.MapFrom(src => src.BeneficiaryName))
                .ForMember(dest => dest.AgreementNo, opt => opt.MapFrom(src => src.AgreementNo))
                .ForMember(dest => dest.Declaration, opt => opt.MapFrom(src => src.Declaration))
                .ForMember(dest => dest.SecurityChequeNo, opt => opt.MapFrom(src => src.SecurityChequeNo))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.SecurityAmount, opt => opt.MapFrom(src => src.SecurityAmount))
                .ForMember(dest => dest.VendorID, opt => opt.MapFrom(src => src.VendorID))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));
         
            CreateMap<TransporterUpdateRequestModel, TransporterUpdateRequest>()
                .ForMember(dest => dest.TransporterID, opt => opt.MapFrom(src => src.TransporterID))
                .ForMember(dest => dest.TransporterName, opt => opt.MapFrom(src => src.TransporterName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNo))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.RegistrationNo, opt => opt.MapFrom(src => src.RegistrationNo))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.LocalContactPerson, opt => opt.MapFrom(src => src.LocalContactPerson))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
                .ForMember(dest => dest.BankAccountNo, opt => opt.MapFrom(src => src.BankAccountNo))
                .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
                .ForMember(dest => dest.GSTIN, opt => opt.MapFrom(src => src.GSTIN))
                .ForMember(dest => dest.TdsPer, opt => opt.MapFrom(src => src.TdsPer))
                .ForMember(dest => dest.PanNo, opt => opt.MapFrom(src => src.PanNo))
                .ForMember(dest => dest.BeneficiaryName, opt => opt.MapFrom(src => src.BeneficiaryName))
                .ForMember(dest => dest.AgreementNo, opt => opt.MapFrom(src => src.AgreementNo))
                .ForMember(dest => dest.Declaration, opt => opt.MapFrom(src => src.Declaration))
                .ForMember(dest => dest.SecurityChequeNo, opt => opt.MapFrom(src => src.SecurityChequeNo))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.SecurityAmount, opt => opt.MapFrom(src => src.SecurityAmount))
                .ForMember(dest => dest.VendorID, opt => opt.MapFrom(src => src.VendorID))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

            CreateMap<VendorInsertRequestModel, VendorInsertRequest>()
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.GSTIN, opt => opt.MapFrom(src => src.GSTIN))
                .ForMember(dest => dest.PanNo, opt => opt.MapFrom(src => src.PanNo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.CreatedBy,opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

          
            CreateMap<VendorUpdateRequestModel, VendorUpdateRequest>()
                 .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.VendorCode, opt => opt.MapFrom(src => src.VendorCode))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.VendorName))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.GSTIN, opt => opt.MapFrom(src => src.GSTIN))
                .ForMember(dest => dest.PanNo, opt => opt.MapFrom(src => src.PanNo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifyBy,opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

            CreateMap<VehicleBillingTypeInsertRequestModel, VehicleBillingTypeInsertRequest>()
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId))
                .ForMember(dest => dest.BillingTypeId, opt => opt.MapFrom(src => src.BillingTypeId))
                .ForMember(dest => dest.WefDate, opt => opt.MapFrom(src => src.WefDate))
                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.TransporterId, opt => opt.MapFrom(src => src.TransporterId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));


            CreateMap<VehicleBillingTypeUpdateRequestModel, VehicleBillingTypeUpdateRequest>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId))
                .ForMember(dest => dest.BillingTypeId, opt => opt.MapFrom(src => src.BillingTypeId))
                .ForMember(dest => dest.WefDate, opt => opt.MapFrom(src => src.WefDate))
                .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.TransporterId, opt => opt.MapFrom(src => src.TransporterId))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));


        }
    }
}
