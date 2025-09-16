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

        public static class MilkCollectionQueries
        {
            public const string AddMilkCollection = "usp_milkcollection_insupd";
            public const string GetMilkCollectionList = "usp_milkcollection_list";
        }

        public static class DockDataQueries
        {
            public const string AddDockData = "usp_dock_data_insupd";
            public const string GetDockDataList = "usp_dock_data_list";
        }

        public static class FarmerStgQueries
        {
            public const string AddFarmerStg = "usp_farmer_collection_staging_insupd";
            public const string GetFarmerStgList = "usp_farmer_staging_export_collection_list";
        }

        public static class FarmerCollectionQueries
        {
            public const string AddFarmerCollection = "usp_farmercollection_insupd";
            public const string GetFarmerCollectionList = "usp_farmercollection_list";
        }
    }
}

