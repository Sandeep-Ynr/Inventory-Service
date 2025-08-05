
using System;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Milk.DeviceSetting
{
    public class DeviceSettingResponse:CommonLists
    {
        public int MppId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? EffectiveShift { get; set; }
        public bool IsManual { get; set; }
        public bool EncryptUsbData { get; set; }
        public string? DpuModel { get; set; }
        public int? MaxCollectionPerShift { get; set; }
        public bool? IsWifiEnabled { get; set; }
        public string? ApName { get; set; }
        public string? ApPassword { get; set; }
        public string? AdminPassword { get; set; }
        public string? SupportPassword { get; set; }
        public string? UserPassword { get; set; }
        public string? Apn { get; set; }
        public bool? IsDispatchMandate { get; set; }
        public bool? IsMaCalibration { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
    }
}
