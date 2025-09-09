using AutoMapper;
using Azure.Core;
using MilkMatrix.Api.Models.Request.Milk;
using MilkMatrix.Api.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Api.Models.Request.Milk.DockData;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerCollection;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Milk;
using MilkMatrix.Milk.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Request.Milk.DockData;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerCollection;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerStagingCollection;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MilkMappingProfile : Profile
    {
        public MilkMappingProfile() 
        {
            CreateMap<MilkTypeUpdateRequestModel, MilkTypeUpdateRequest>()
                        .ForMember(x => x.MilkTypeId, opt => opt.MapFrom(src => src.MilkTypeId))
                        .ForMember(x => x.MilkTypeName, opt => opt.MapFrom(src => src.MilkTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<MilkTypeInsertRequestModel, MilkTypeInsertRequest>()
                        .ForMember(x => x.MilkTypeName, opt => opt.MapFrom(src => src.MilkTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
                    
            CreateMap<RateTypeUpdateRequestModel, RateTypeUpdateRequest>()
                        .ForMember(x => x.RateTypeId, opt => opt.MapFrom(src => src.RateTypeId))
                        .ForMember(x => x.RateTypeName, opt => opt.MapFrom(src => src.RateTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<RateTypeInsertRequestModel, RateTypeInsertRequest>()
                        .ForMember(x => x.RateTypeName, opt => opt.MapFrom(src => src.RateTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

            CreateMap<MeasurementUnitUpdateModel, MeasurementUnitUpdateRequest>()
                        .ForMember(x => x.MeasurementUnitId, opt => opt.MapFrom(src => src.MeasurementUnitId))
                        .ForMember(x => x.MeasurementUnitCode, opt => opt.MapFrom(src => src.MeasurementUnitCode))
                        .ForMember(x => x.MeasurementUnitName, opt => opt.MapFrom(src => src.MeasurementUnitName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<MeasurementUnitInsertModel, MeasurementUnitInsertRequest>()
                        .ForMember(x => x.MeasurementUnitCode, opt => opt.MapFrom(src => src.MeasurementUnitCode))
                        .ForMember(x => x.MeasurementUnitName, opt => opt.MapFrom(src => src.MeasurementUnitName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

            CreateMap<MilkCollectionInsertRequestModel, MilkCollectionInsertRequest>()
                         .ForMember(dest => dest.BusinessID, opt => opt.MapFrom(src => src.BusinessID))
                         .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId))
                         .ForMember(dest => dest.CenterType, opt => opt.MapFrom(src => src.CenterType))
                         .ForMember(dest => dest.CenterId, opt => opt.MapFrom(src => src.CenterId))
                         .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                         .ForMember(dest => dest.CollectionDate, opt => opt.MapFrom(src => src.CollectionDate))
                         .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                         .ForMember(dest => dest.MilkType, opt => opt.MapFrom(src => src.MilkType))
                         .ForMember(dest => dest.QuantityLtr, opt => opt.MapFrom(src => src.QuantityLtr))
                         .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
                         .ForMember(dest => dest.Snf, opt => opt.MapFrom(src => src.Snf))
                         .ForMember(dest => dest.RatePerLtr, opt => opt.MapFrom(src => src.RatePerLtr))
                         .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                         .ForMember(dest => dest.CollectionMode, opt => opt.MapFrom(src => src.CollectionMode))
                         .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                         .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                         .ForMember(dest => dest.BusinessID, opt => opt.MapFrom(src => src.BusinessID))
                         .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));

           


            CreateMap<DeviceSettingInsertRequestModel, DeviceSettingInsertRequest>()
                      .ForMember(dest => dest.MppId, opt => opt.MapFrom(src => src.MppId))
                      .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate))
                      .ForMember(dest => dest.EffectiveShift, opt => opt.MapFrom(src => src.EffectiveShift))
                      .ForMember(dest => dest.IsManual, opt => opt.MapFrom(src => src.IsManual))
                      .ForMember(dest => dest.EncryptUsbData, opt => opt.MapFrom(src => src.EncryptUsbData))
                      .ForMember(dest => dest.DpuModel, opt => opt.MapFrom(src => src.DpuModel))
                      .ForMember(dest => dest.MaxCollectionPerShift, opt => opt.MapFrom(src => src.MaxCollectionPerShift))
                      .ForMember(dest => dest.IsWifiEnabled, opt => opt.MapFrom(src => src.IsWifiEnabled))
                      .ForMember(dest => dest.ApName, opt => opt.MapFrom(src => src.ApName))
                      .ForMember(dest => dest.ApPassword, opt => opt.MapFrom(src => src.ApPassword))
                      .ForMember(dest => dest.AdminPassword, opt => opt.MapFrom(src => src.AdminPassword))
                      .ForMember(dest => dest.SupportPassword, opt => opt.MapFrom(src => src.SupportPassword))
                      .ForMember(dest => dest.UserPassword, opt => opt.MapFrom(src => src.UserPassword))
                      .ForMember(dest => dest.Apn, opt => opt.MapFrom(src => src.Apn))
                      .ForMember(dest => dest.IsDispatchMandate, opt => opt.MapFrom(src => src.IsDispatchMandate))
                      .ForMember(dest => dest.IsMaCalibration, opt => opt.MapFrom(src => src.IsMaCalibration))
                      .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.is_status))
                      .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));


            CreateMap<MilkCollectionUpdateRequestModel, MilkCollectionUpdateRequest>()
                        .ForMember(dest => dest.BusinessID, opt => opt.MapFrom(src => src.BusinessID))
                        .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId))
                        .ForMember(dest => dest.CenterType, opt => opt.MapFrom(src => src.CenterType))
                        .ForMember(dest => dest.CenterId, opt => opt.MapFrom(src => src.CenterId))
                        .ForMember(dest => dest.RouteId, opt => opt.MapFrom(src => src.RouteId))
                        .ForMember(dest => dest.CollectionDate, opt => opt.MapFrom(src => src.CollectionDate))
                        .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                        .ForMember(dest => dest.MilkType, opt => opt.MapFrom(src => src.MilkType))
                        .ForMember(dest => dest.QuantityLtr, opt => opt.MapFrom(src => src.QuantityLtr))
                        .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
                        .ForMember(dest => dest.Snf, opt => opt.MapFrom(src => src.Snf))
                        .ForMember(dest => dest.RatePerLtr, opt => opt.MapFrom(src => src.RatePerLtr))
                        .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                        .ForMember(dest => dest.CollectionMode, opt => opt.MapFrom(src => src.CollectionMode))
                        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                        .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(dest => dest.BusinessID, opt => opt.MapFrom(src => src.BusinessID))
                        .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));


            CreateMap<DeviceSettingUpdateRequestModel, DeviceSettingUpdateRequest>()
                        .ForMember(dest => dest.DeviceSettingId, opt => opt.MapFrom(src => src.DeviceSettingId))
                        .ForMember(dest => dest.MppId, opt => opt.MapFrom(src => src.MppId))
                        .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate))
                        .ForMember(dest => dest.EffectiveShift, opt => opt.MapFrom(src => src.EffectiveShift))
                        .ForMember(dest => dest.IsManual, opt => opt.MapFrom(src => src.IsManual))
                        .ForMember(dest => dest.EncryptUsbData, opt => opt.MapFrom(src => src.EncryptUsbData))
                        .ForMember(dest => dest.DpuModel, opt => opt.MapFrom(src => src.DpuModel))
                        .ForMember(dest => dest.MaxCollectionPerShift, opt => opt.MapFrom(src => src.MaxCollectionPerShift))
                        .ForMember(dest => dest.IsWifiEnabled, opt => opt.MapFrom(src => src.IsWifiEnabled))
                        .ForMember(dest => dest.ApName, opt => opt.MapFrom(src => src.ApName))
                        .ForMember(dest => dest.ApPassword, opt => opt.MapFrom(src => src.ApPassword))
                        .ForMember(dest => dest.AdminPassword, opt => opt.MapFrom(src => src.AdminPassword))
                        .ForMember(dest => dest.SupportPassword, opt => opt.MapFrom(src => src.SupportPassword))
                        .ForMember(dest => dest.UserPassword, opt => opt.MapFrom(src => src.UserPassword))
                        .ForMember(dest => dest.Apn, opt => opt.MapFrom(src => src.Apn))
                        .ForMember(dest => dest.IsDispatchMandate, opt => opt.MapFrom(src => src.IsDispatchMandate))
                        .ForMember(dest => dest.IsMaCalibration, opt => opt.MapFrom(src => src.IsMaCalibration))
                        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.is_status))
                        .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));
        

                    CreateMap<DockDataInsertRequestModel, DockDataInsertRequest>()
                        .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        .ForMember(dest => dest.BmcId, opt => opt.MapFrom(src => src.BmcId))
                        .ForMember(dest => dest.DumpDate, opt => opt.MapFrom(src => src.DumpDate))
                        .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                        .ForMember(dest => dest.UpdateStatus, opt => opt.MapFrom(src => src.UpdateStatus))
                        .ForMember(dest => dest.UpdatedRecords, opt => opt.MapFrom(src => src.UpdatedRecords))
                        .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                        .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsActive));



            CreateMap<DockDataUpdateRequestModel, DockDataRequest>()
                        .ForMember(dest => dest.DockDataUpdateId, opt => opt.MapFrom(src => src.DockDataUpdateId))
                        .ForMember(dest => dest.BmcId, opt => opt.MapFrom(src => src.BmcId))
                        .ForMember(dest => dest.DumpDate, opt => opt.MapFrom(src => src.DumpDate))
                        .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                        .ForMember(dest => dest.UpdateStatus, opt => opt.MapFrom(src => src.UpdateStatus))
                        .ForMember(dest => dest.UpdatedRecords, opt => opt.MapFrom(src => src.UpdatedRecords))
                        .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks))
                        .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsActive))
                        .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));


            CreateMap<FarmerCollStgInsertRequestModel, FarmerCollStgInsertRequest>()
                        //.ForMember(dest => dest.HeaderId, opt => opt.MapFrom(src => src.HeaderId))
                        .ForMember(dest => dest.DumpDate, opt => opt.MapFrom(src => src.DumpDate))
                        .ForMember(dest => dest.DumpTime, opt => opt.MapFrom(src => src.DumpTime))
                        .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                        .ForMember(dest => dest.BatchNo, opt => opt.MapFrom(src => src.BatchNo))
                        .ForMember(dest => dest.MppId, opt => opt.MapFrom(src => src.MppId))
                        .ForMember(dest => dest.BmcID, opt => opt.MapFrom(src => src.BmcID))
                        .ForMember(dest => dest.InsertMode, opt => opt.MapFrom(src => src.InsertMode))
                        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                        .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                        .ForMember(dest => dest.ImeiNo, opt => opt.MapFrom(src => src.ImeiNo))
                        .ForMember(dest => dest.IsValidated, opt => opt.MapFrom(src => src.IsValidated))
                        .ForMember(dest => dest.IsProcess, opt => opt.MapFrom(src => src.IsProcess))
                        .ForMember(dest => dest.ProcessDate, opt => opt.MapFrom(src => src.ProcessDate))
                        .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                        //.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn ?? DateTime.Now))
                        //.ForMember(dest => dest.ModifyBy, opt => opt.MapFrom(src => src.ModifyBy))
                        //.ForMember(dest => dest.ModifyOn, opt => opt.MapFrom(src => src.ModifyOn))
                        .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                        .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));


            CreateMap<FarmerCollectionStagingDetailModel, FarmerCollectionStagingDetail>()
                  .ForMember(dest => dest.FarmerCode, opt => opt.MapFrom(src => src.FarmerCode))
                  .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                  .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
                  .ForMember(dest => dest.Snf, opt => opt.MapFrom(src => src.Snf))
                  .ForMember(dest => dest.LR, opt => opt.MapFrom(src => src.LR))
                  .ForMember(dest => dest.WeightLiter, opt => opt.MapFrom(src => src.WeightLiter))
                  .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                  .ForMember(dest => dest.Rtpl, opt => opt.MapFrom(src => src.Rptl))
                  .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                  .ForMember(dest => dest.SampleId, opt => opt.MapFrom(src => src.SampleId))
                  .ForMember(dest => dest.Can, opt => opt.MapFrom(src => src.Can))
                  .ForMember(dest => dest.ReferenceId, opt => opt.MapFrom(src => src.ReferenceId));

            CreateMap<FarmerCollectionInsertRequestModel, FarmerCollectionInsertRequest>()
                    .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                    .ForMember(dest => dest.DumpDate, opt => opt.MapFrom(src => src.DumpDate))
                    .ForMember(dest => dest.DumpTime, opt => opt.MapFrom(src => src.DumpTime))
                    .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => src.FarmerId))
                    .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
                    .ForMember(dest => dest.Snf, opt => opt.MapFrom(src => src.Snf))
                    .ForMember(dest => dest.LR, opt => opt.MapFrom(src => src.LR))
                    .ForMember(dest => dest.WeightLiter, opt => opt.MapFrom(src => src.WeightLiter))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Rtpl, opt => opt.MapFrom(src => src.Rtpl))
                    .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                    .ForMember(dest => dest.SampleId, opt => opt.MapFrom(src => src.SampleId))
                    .ForMember(dest => dest.BatchNo, opt => opt.MapFrom(src => src.BatchNo))
                    .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                    .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                    .ForMember(dest => dest.IMEI_No, opt => opt.MapFrom(src => src.IMEI_No))
                    .ForMember(dest => dest.BmcId, opt => opt.MapFrom(src => src.BmcId))
                    .ForMember(dest => dest.MccId, opt => opt.MapFrom(src => src.MccId))
                    .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ForMember(dest => dest.CId, opt => opt.MapFrom(src => src.CId))
                    .ForMember(dest => dest.CDate, opt => opt.MapFrom(src => src.CDate))
                    .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy))
                    .ForMember(dest => dest.ApprovedDate, opt => opt.MapFrom(src => src.ApprovedDate))
                    .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            
            CreateMap<FarmerCollectionUpdateRequestModel, FarmerCollectionUpdateRequest>()
                .ForMember(dest => dest.FarmerCollectionId, opt => opt.MapFrom(src => src.FarmerCollectionId))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.DumpDate, opt => opt.MapFrom(src => src.DumpDate))
                .ForMember(dest => dest.DumpTime, opt => opt.MapFrom(src => src.DumpTime))
                .ForMember(dest => dest.FarmerId, opt => opt.MapFrom(src => src.FarmerId))
                .ForMember(dest => dest.Fat, opt => opt.MapFrom(src => src.Fat))
                .ForMember(dest => dest.Snf, opt => opt.MapFrom(src => src.Snf))
                .ForMember(dest => dest.LR, opt => opt.MapFrom(src => src.LR))
                .ForMember(dest => dest.WeightLiter, opt => opt.MapFrom(src => src.WeightLiter))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Rtpl, opt => opt.MapFrom(src => src.Rtpl))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.SampleId, opt => opt.MapFrom(src => src.SampleId))
                .ForMember(dest => dest.BatchNo, opt => opt.MapFrom(src => src.BatchNo))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.IMEI_No, opt => opt.MapFrom(src => src.IMEI_No))
                .ForMember(dest => dest.BmcId, opt => opt.MapFrom(src => src.BmcId))
                .ForMember(dest => dest.MccId, opt => opt.MapFrom(src => src.MccId))
                .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.CId, opt => opt.MapFrom(src => src.CId))
                .ForMember(dest => dest.CDate, opt => opt.MapFrom(src => src.CDate))
                .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy))
                .ForMember(dest => dest.ApprovedDate, opt => opt.MapFrom(src => src.ApprovedDate))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
        }

    }
}
