namespace MilkMatrix.Api.Models.Request.Admin.GlobleSetting.ConfigSettings
{
    public class ConfigSettingUpdateRequestModel
    {
        public int? BusinessId { get; set; }
        public int CompanyId { get; set; }   // Primary Key, Identity
        public string UnitType { get; set; }
        public string? UnitId { get; set; }
        public int? GenlCanPerLit { get; set; }
        public int? GenlCanWarThr { get; set; }
        public decimal? GenlLtrToKgConFac { get; set; }
        public bool? GenlWeiRouMod { get; set; }
        public bool? GenlQuaRouMod { get; set; }
        public string GenlMemQuaMod { get; set; }
        public string GenlBmcQuaMod { get; set; }
        public bool? GenlUseDefSnf { get; set; }
        public decimal? GenlDefSnfVal { get; set; }
        public bool? GenlZerRatAll { get; set; }
        public bool? GenlEnaShiWisEntDat { get; set; }
        public decimal? QcdlColMinFat { get; set; }
        public decimal? QcdlColMaxFat { get; set; }
        public decimal? QcdlColMinSnf { get; set; }
        public decimal? QcdlColMaxSnf { get; set; }
        public string QcdlDisQtyMod { get; set; }
        public decimal? QcdlDisLtrKg { get; set; }
        public decimal? QcdlDisMinFat { get; set; }
        public decimal? QcdlDisMaxFat { get; set; }
        public decimal? QcdlDisMinSnf { get; set; }
        public decimal? QcdlDisMaxSnf { get; set; }
        public string DvalMilTypDva { get; set; }
        public decimal? DvalDefFat { get; set; }
        public decimal? DvalDefSnf { get; set; }
        public bool? WsetIsActWse { get; set; }
        public string WsetWeiRouBy { get; set; }
        public string WsetCanAveWei { get; set; }
        public int? LsetTraSiz { get; set; }
        public bool? LsetSnfRou { get; set; }
        public string LsetSnfRouBy { get; set; }
        public int? LsetSnfAftDec { get; set; }
        public string LsetSnfFor { get; set; }
        public decimal? LsetSnfCon { get; set; }
        public bool? LsetFatRou { get; set; }
        public string LsetFatRouBy { get; set; }
        public int? LsetFatAftDec { get; set; }
        public bool? LsetLrRou { get; set; }
        public string LsetLrRouBy { get; set; }
        public int? LsetLrAftDec { get; set; }
        public string QlmtMilTypQlm { get; set; }
        public decimal? QlmtMinFat { get; set; }
        public decimal? QlmtMaxFat { get; set; }
        public decimal? QlmtMinSnf { get; set; }
        public decimal? QlmtMaxSnf { get; set; }
        public decimal? QlmtMinClr { get; set; }
        public decimal? QlmtMaxClr { get; set; }
        public string AdltTesNam { get; set; }
        public bool? AdltIsEnaAdl { get; set; }
        public bool? FuncAllFarCodEdi { get; set; }
        public bool? FuncValRatRanOnImp { get; set; }
        public bool? FuncLoaAllRouOnDoc { get; set; }
        public bool? FuncAllDupFarCol { get; set; }
        public bool? FuncEnaPayCyc { get; set; }
        public string UnitBmcColMod { get; set; }
        public string UnitMppColMod { get; set; }
        public decimal? UnitLitToKgCon { get; set; }
        public decimal? UnitKgToLitCon { get; set; }
        public string EntrMulSamTyp { get; set; }
        public string EntrMulDifTyp { get; set; }
        public string InptInpFat { get; set; }
        public string InptInpSnf { get; set; }
        public string InptInpClr { get; set; }
        public string InptInpPro { get; set; }
        public string InptInpLac { get; set; }
        public string InptInpWat { get; set; }
        public decimal? VariVarFat { get; set; }
        public bool? VariBloFat { get; set; }
        public decimal? VariVarSnf { get; set; }
        public bool? VariBloSnf { get; set; }
        public decimal? VariVarQty { get; set; }
        public bool? VariBloQty { get; set; }
        public string MachMacBasOn { get; set; }
        public string MachMacShi { get; set; }
        public string MachMacNo { get; set; }
        public string DsplDisBasOn { get; set; }
        public string DsplDisShi { get; set; }
        public string DsplDisNo { get; set; }
        public bool? OthrIsAutMer { get; set; }
        public int? OthrRatChaApp { get; set; }
        public bool? IsStatus { get; set; }

        public int? ModifiedBy { get; set; }

    }
}
