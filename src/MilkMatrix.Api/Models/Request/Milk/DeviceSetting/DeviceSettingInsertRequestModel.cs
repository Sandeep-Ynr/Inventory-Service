namespace MilkMatrix.Api.Models.Request.Milk.DeviceSetting
{
    public class DeviceSettingInsertRequestModel
    {
        public string? BusinessId { get; set; }
        public int MppId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? EffectiveShift { get; set; }
        public bool IsManual { get; set; }
        public bool EncryptUsbData { get; set; }
        public string? DpuModel { get; set; }
        public int MaxCollectionPerShift { get; set; }
        public bool IsWifiEnabled { get; set; }
        public string? ApName { get; set; }
        public string? ApPassword { get; set; }
        public string? AdminPassword { get; set; }
        public string? SupportPassword { get; set; }
        public string? UserPassword { get; set; }
        public string? Apn { get; set; }
        public bool IsDispatchMandate { get; set; }
        public bool IsMaCalibration { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }

    }
}
