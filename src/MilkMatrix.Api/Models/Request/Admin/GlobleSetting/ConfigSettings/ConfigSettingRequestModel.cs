namespace MilkMatrix.Api.Models.Request.Admin.GlobleSetting.ConfigSettings
{
    public class ConfigSettingRequestModel
    {
        public int BusinessId { get; set; }
        public int CompanyId { get; set; }
        public string UnitType { get; set; }
        public int? UnitId { get; set; }

        // General Settings (GENL)
        public decimal? GenlCanPerLit { get; set; }
        public decimal? GenlLtrToKgConFac { get; set; }
        public string GenlRoundMode { get; set; }
        public bool? GenlUseDefSnf { get; set; }
        public decimal? GenlDefSnf { get; set; }

        // Quality Collection/Dispatch Limits (QCDL)
        public decimal? QcdlColMinFat { get; set; }
        public decimal? QcdlColMaxFat { get; set; }
        public decimal? QcdlDisMinSnf { get; set; }
        public decimal? QcdlDisMaxSnf { get; set; }

        // Default Values (DVAL)
        public decimal? DvalDefFat { get; set; }
        public decimal? DvalDefSnf { get; set; }
        public string DvalMilkType { get; set; }

        // Weight Settings (WSET)
        public string WsetRoundWtMode { get; set; }
        public decimal? WsetAvgCanWt { get; set; }

        // Lab Settings (LSET)
        public int? LsetSnfRou { get; set; }
        public int? LsetFatAftDec { get; set; }
        public int? LsetClrAftDec { get; set; }

        // Quality Limits (QLMT)
        public decimal? QlmtMinFat { get; set; }
        public decimal? QlmtMaxFat { get; set; }
        public decimal? QlmtMinSnf { get; set; }
        public decimal? QlmtMaxSnf { get; set; }
        public decimal? QlmtMinClr { get; set; }
        public decimal? QlmtMaxClr { get; set; }

        // Adulteration Settings (ADLT)
        public bool? AdltMbrAllow { get; set; }
        public bool? AdltBlock { get; set; }

        // Functional Toggles (FUNC)
        public bool? FuncCollWhileUnapprove { get; set; }
        public bool? FuncDirectDispatch { get; set; }

        // Unit Conversion (UNIT)
        public decimal? UnitKgToLtrFactor { get; set; }

        // Entry Rules (ENTR)
        public bool? EntrSameMilkType { get; set; }
        public bool? EntrDiffMilkType { get; set; }

        // Input Toggles (INPT)
        public bool? InptFatEditable { get; set; }
        public bool? InptSnfEditable { get; set; }

        // Variations (VARI)
        public decimal? VariVarFat { get; set; }
        public decimal? VariVarSnf { get; set; }
        public decimal? VariBloQty { get; set; }

        // Machine Settings (MACH)
        public string MachMacNo { get; set; }
        public string MachMacShi { get; set; }

        // Display (DSPL)
        public string DsplMapRule { get; set; }

        // Miscellaneous (OTHR)
        public string OthrNotes { get; set; }
    }
}

