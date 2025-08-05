using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public static partial class MilkQueries
    {
        public static class MilkTypeQueries
        {
            public const string GetMilkTypeList = "usp_milk_type_list";
            public const string InsupdMilkType = "usp_milk_type_insupd";
        }

        public static class RateTypeQueries
        {
            public const string GetRateTypeList = "usp_rate_type_list";
            public const string InsupdRateType = "usp_rate_type_insupd";
        }

        public static class MeasurementUnitQueries
        {
            public const string GetMeasurementUnitList = "usp_measurement_unit_list";
            public const string InsupdMeasurementUnit = "usp_measurement_unit_insupd";
        }
        public static class DeviceSettingQueries
        {
            public const string AddDeviceSetting = "usp_device_settings_insupd";
            public const string GetDeviceSettingList = "usp_device_settings_list";
        }
    }
}
