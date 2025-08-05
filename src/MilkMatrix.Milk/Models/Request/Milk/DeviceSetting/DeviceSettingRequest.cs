
using System;

namespace MilkMatrix.Milk.Models.Request.Milk.DeviceSetting
{
    public class DeviceSettingRequest
    {
        public int DeviceSettingId { get; set; }
        public string MppId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EffectiveShift { get; set; }
        public bool IsManual { get; set; }
        public bool EncryptUsbData { get; set; }
        public string DpuModel { get; set; }
        public int MaxCollectionPerShift { get; set; }
        public bool IsWifiEnabled { get; set; }
        public string ApName { get; set; }
        public string ApPassword { get; set; }
        public string AdminPassword { get; set; }
        public string SupportPassword { get; set; }
        public string UserPassword { get; set; }
        public string Apn { get; set; }
        public bool IsDispatchMandate { get; set; }
        public bool IsMaCalibration { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
