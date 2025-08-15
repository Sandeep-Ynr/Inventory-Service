using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public static class RouteQueries
    {
        public const string AddRoute = "usp_route_insupd";
        public const string GetRouteList = "usp_route_list";
    }
    public static class VehicleTypeQueries
    {
        public const string AddVehicle = "usp_vehicletype_insupd";
        public const string GetVehicleList = "usp_vehicletype_list";
    }
    public static class TransporterQueries
    {
        public const string AddTransporter = "usp_transporter_insupd";
        public const string GetTransporterList = "usp_transporter_list";
    }
    public static class VehicleQueries
    {
        public const string AddVehicle = "usp_vehicle_insupd";
        public const string GetVehicleList = "usp_vehicle_list";
    }
    public static class VendorQueries
    {
        public const string AddVendor = "usp_vendor_insupd";
        public const string GetVendorList = "usp_vendor_list";
    }

    public static class VehicleBillingTypeQueries
    {
        public const string VehicleBilling = "usp_VehicleBillingType_InsUpd";
        public const string VehicleBillingList = "usp_vehiclebillingtype_list";
    }

    public static class RouteContractorQueries
    {
        public const string AddRouteContractor = "usp_routecontractor_insupd";
        public const string GetRouteContractorList = "usp_routecontractor_list";
    }

    public static class RouteTimeQueries
    {
        public const string AddUpdateRouteTiming = "usp_route_schedule_insupd";
        public const string GetRouteTimingList = "usp_route_schedule_list";
    }

}
