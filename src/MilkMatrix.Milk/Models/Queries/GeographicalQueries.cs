namespace MilkMatrix.Milk.Models.Queries;

public static partial class GeographicalQueries
{
    public static class StateQueries
    {
        public const string GetStates = "usp_state_details";
        public const string AddStates = "usp_state_insupd";
        
    }

    public static class DistrictQueries
    {
        public const string GetDistrict = "usp_district_details";
        public const string AddDistrict = "usp_district_insupd";
    }
    public static class TehsilQueries
    {
        public const string GetTehsil = "usp_tehsil_details";
        public const string AddTehsil = "usp_tehsil_insupd";
    }

    public static class VillageQueries 
    { 
        public const string GetVillage = "usp_village_details";
    }

    public static class HamletQueries
    {
        public const string GetHamlet = "usp_hamlet_details";
    }

}
