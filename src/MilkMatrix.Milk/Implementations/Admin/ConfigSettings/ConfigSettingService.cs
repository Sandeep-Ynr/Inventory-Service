using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Dapper;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.ConfigSettings;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.ConfigSettings;
using MilkMatrix.Milk.Models.Response.ConfigSettings;

namespace MilkMatrix.Milk.Implementations.ConfigSettings
{
    public class ConfigSettingService : IConfigSettingService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public ConfigSettingService(
            ILogging logging,
            IOptions<AppConfig> appConfig,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(ConfigSettingService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }


        public async Task InsertConfigSetting(ConfigSettingInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "BusinessId", request.BusinessId },
                    { "CompanyId", request.CompanyId },
                    { "UnitType", request.UnitType },
                    { "UnitId", request.UnitId ?? (object)DBNull.Value  },
                    { "genl_can_per_lit", request.GenlCanPerLit ?? (object)DBNull.Value },
                    { "genl_can_war_thr", request.GenlCanWarThr ?? (object)DBNull.Value },
                    { "genl_ltr_to_kg_con_fac", request.GenlLtrToKgConFac ?? (object)DBNull.Value },
                    { "genl_wei_rou_mod", request.GenlWeiRouMod ?? (object)DBNull.Value },
                    { "genl_qua_rou_mod", request.GenlQuaRouMod ?? (object)DBNull.Value },
                    { "genl_mem_qua_mod", request.GenlMemQuaMod ?? (object)DBNull.Value },
                    { "genl_bmc_qua_mod", request.GenlBmcQuaMod ?? (object)DBNull.Value },
                    { "genl_use_def_snf", request.GenlUseDefSnf ?? (object)DBNull.Value },
                    { "genl_def_snf_val", request.GenlDefSnfVal ?? (object)DBNull.Value },
                    { "genl_zer_rat_all", request.GenlZerRatAll ?? (object)DBNull.Value },
                    { "genl_ena_shi_wis_ent_dat", request.GenlEnaShiWisEntDat ?? (object)DBNull.Value },
                    { "qcdl_col_min_fat", request.QcdlColMinFat ?? (object)DBNull.Value },
                    { "qcdl_col_max_fat", request.QcdlColMaxFat ?? (object)DBNull.Value },
                    { "qcdl_col_min_snf", request.QcdlColMinSnf ?? (object)DBNull.Value },
                    { "qcdl_col_max_snf", request.QcdlColMaxSnf ?? (object)DBNull.Value },
                    { "qcdl_dis_qty_mod", request.QcdlDisQtyMod ?? (object)DBNull.Value },
                    { "qcdl_dis_ltr_kg", request.QcdlDisLtrKg ?? (object)DBNull.Value },
                    { "qcdl_dis_min_fat", request.QcdlDisMinFat ?? (object)DBNull.Value },
                    { "qcdl_dis_max_fat", request.QcdlDisMaxFat ?? (object)DBNull.Value },
                    { "qcdl_dis_min_snf", request.QcdlDisMinSnf ?? (object)DBNull.Value },
                    { "qcdl_dis_max_snf", request.QcdlDisMaxSnf ?? (object)DBNull.Value },
                    { "dval_mil_typ_dva", request.DvalMilTypDva ?? (object)DBNull.Value },
                    { "dval_def_fat", request.DvalDefFat ?? (object)DBNull.Value },
                    { "dval_def_snf", request.DvalDefSnf ?? (object)DBNull.Value },
                    { "wset_is_act_wse", request.WsetIsActWse ?? (object)DBNull.Value },
                    { "wset_wei_rou_by", request.WsetWeiRouBy ?? (object)DBNull.Value },
                    { "wset_can_ave_wei", request.WsetCanAveWei ?? (object)DBNull.Value },
                    { "lset_tra_siz", request.LsetTraSiz ?? (object)DBNull.Value },
                    { "lset_snf_rou", request.LsetSnfRou ?? (object)DBNull.Value },
                    { "lset_snf_rou_by", request.LsetSnfRouBy ?? (object)DBNull.Value },
                    { "lset_snf_aft_dec", request.LsetSnfAftDec ?? (object)DBNull.Value },
                    { "lset_snf_for", request.LsetSnfFor ?? (object)DBNull.Value },
                    { "lset_snf_con", request.LsetSnfCon ?? (object)DBNull.Value },
                    { "lset_fat_rou", request.LsetFatRou ?? (object)DBNull.Value },
                    { "lset_fat_rou_by", request.LsetFatRouBy ?? (object)DBNull.Value },
                    { "lset_fat_aft_dec", request.LsetFatAftDec ?? (object)DBNull.Value },
                    { "lset_lr_rou", request.LsetLrRou ?? (object)DBNull.Value },
                    { "lset_lr_rou_by", request.LsetLrRouBy ?? (object)DBNull.Value },
                    { "lset_lr_aft_dec", request.LsetLrAftDec ?? (object)DBNull.Value },
                    { "qlmt_mil_typ_qlm", request.QlmtMilTypQlm ?? (object)DBNull.Value },
                    { "qlmt_min_fat", request.QlmtMinFat ?? (object)DBNull.Value },
                    { "qlmt_max_fat", request.QlmtMaxFat ?? (object)DBNull.Value },
                    { "qlmt_min_snf", request.QlmtMinSnf ?? (object)DBNull.Value },
                    { "qlmt_max_snf", request.QlmtMaxSnf ?? (object)DBNull.Value },
                    { "qlmt_min_clr", request.QlmtMinClr ?? (object)DBNull.Value },
                    { "qlmt_max_clr", request.QlmtMaxClr ?? (object)DBNull.Value },
                    { "adlt_tes_nam", request.AdltTesNam ?? (object)DBNull.Value },
                    { "adlt_is_ena_adl", request.AdltIsEnaAdl ?? (object)DBNull.Value },
                    { "func_all_far_cod_edi", request.FuncAllFarCodEdi ?? (object)DBNull.Value },
                    { "func_val_rat_ran_on_imp", request.FuncValRatRanOnImp ?? (object)DBNull.Value },
                    { "func_loa_all_rou_on_doc", request.FuncLoaAllRouOnDoc ?? (object)DBNull.Value },
                    { "func_all_dup_far_col", request.FuncAllDupFarCol ?? (object)DBNull.Value },
                    { "func_ena_pay_cyc", request.FuncEnaPayCyc ?? (object)DBNull.Value },
                    { "unit_bmc_col_mod", request.UnitBmcColMod ?? (object)DBNull.Value },
                    { "unit_mpp_col_mod", request.UnitMppColMod ?? (object)DBNull.Value },
                    { "unit_lit_to_kg_con", request.UnitLitToKgCon ?? (object)DBNull.Value },
                    { "unit_kg_to_lit_con", request.UnitKgToLitCon ?? (object)DBNull.Value },
                    { "entr_mul_sam_typ", request.EntrMulSamTyp ?? (object)DBNull.Value },
                    { "entr_mul_dif_typ", request.EntrMulDifTyp ?? (object)DBNull.Value },
                    { "inpt_inp_fat", request.InptInpFat ?? (object)DBNull.Value },
                    { "inpt_inp_snf", request.InptInpSnf ?? (object)DBNull.Value },
                    { "inpt_inp_clr", request.InptInpClr ?? (object)DBNull.Value },
                    { "inpt_inp_pro", request.InptInpPro ?? (object)DBNull.Value },
                    { "inpt_inp_lac", request.InptInpLac ?? (object)DBNull.Value },
                    { "inpt_inp_wat", request.InptInpWat ?? (object)DBNull.Value },
                    { "vari_var_fat", request.VariVarFat ?? (object)DBNull.Value },
                    { "vari_blo_fat", request.VariBloFat ?? (object)DBNull.Value },
                    { "vari_var_snf", request.VariVarSnf ?? (object)DBNull.Value },
                    { "vari_blo_snf", request.VariBloSnf ?? (object)DBNull.Value },
                    { "vari_var_qty", request.VariVarQty ?? (object)DBNull.Value },
                    { "vari_blo_qty", request.VariBloQty ?? (object)DBNull.Value },
                    { "mach_mac_bas_on", request.MachMacBasOn ?? (object)DBNull.Value },
                    { "mach_mac_shi", request.MachMacShi ?? (object)DBNull.Value },
                    { "mach_mac_no", request.MachMacNo ?? (object)DBNull.Value },
                    { "dspl_dis_bas_on", request.DsplDisBasOn ?? (object)DBNull.Value },
                    { "dspl_dis_shi", request.DsplDisShi ?? (object)DBNull.Value },
                    { "dspl_dis_no", request.DsplDisNo ?? (object)DBNull.Value },
                    { "othr_is_aut_mer", request.OthrIsAutMer ?? (object)DBNull.Value },
                    { "othr_rat_cha_app", request.OthrRatChaApp ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value }
                };

            var message = await repository.AddAsync(ConfigSettingQueries.AddConfigSetting, requestParams, CommandType.StoredProcedure);
            if (message.StartsWith("Error"))
            {
                throw new Exception($"Stored Procedure Error: {message}");
            }
            else
            {
                logging.LogInfo($"Config Setting {message} added successfully.");
            }
        }
            catch (Exception ex)
            {
                logging.LogError($"Error in InsertConfigSetting for BusinessId: {request.BusinessId}", ex);
                throw;
            }
        }

        public async Task UpdateConfigSetting(ConfigSettingUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "ConfigId", request.ConfigId },
                    { "BusinessId", request.BusinessId },
                    { "CompanyId", request.CompanyId },
                    { "UnitType", request.UnitType },
                    { "UnitId", request.UnitId ?? (object)DBNull.Value},
                    { "GenlCanPerLit", request.GenlCanPerLit ?? (object)DBNull.Value},
                    { "GenlLtrToKgConFac", request.GenlLtrToKgConFac ?? (object)DBNull.Value},
                    { "GenlRoundMode", request.GenlRoundMode ?? (object)DBNull.Value},
                    { "GenlUseDefSnf", request.GenlUseDefSnf ?? (object)DBNull.Value},
                    { "GenlDefSnfVal", request.GenlDefSnf ?? (object)DBNull.Value},
                    { "QcdlColMinFat", request.QcdlColMinFat ?? (object)DBNull.Value},
                    { "QcdlColMaxFat", request.QcdlColMaxFat ?? (object)DBNull.Value},
                    { "QcdlDisMinSnf", request.QcdlDisMinSnf ?? (object)DBNull.Value},
                    { "QcdlDisMaxSnf", request.QcdlDisMaxSnf ?? (object)DBNull.Value},
                    { "DvalDefFat", request.DvalDefFat ?? (object)DBNull.Value},
                    { "DvalDefSnf", request.DvalDefSnf ?? (object)DBNull.Value},
                    { "DvalMilTypDva", request.DvalMilkType ?? (object)DBNull.Value},
                    { "WsetWeiRouBy", request.WsetRoundWtMode ?? (object)DBNull.Value},
                    { "WsetCanAveWei", request.WsetAvgCanWt ?? (object)DBNull.Value},
                    { "LsetSnfRou", request.LsetSnfRou ?? (object)DBNull.Value},
                    { "LsetFatAftDec", request.LsetFatAftDec ?? (object)DBNull.Value},
                    { "LsetLrAftDec", request.LsetClrAftDec ?? (object)DBNull.Value},
                    { "QlmtMinFat", request.QlmtMinFat ?? (object)DBNull.Value},
                    { "QlmtMaxFat", request.QlmtMaxFat ?? (object)DBNull.Value},
                    { "QlmtMinSnf", request.QlmtMinSnf ?? (object)DBNull.Value},
                    { "QlmtMaxSnf", request.QlmtMaxSnf ?? (object)DBNull.Value},
                    { "QlmtMinClr", request.QlmtMinClr ?? (object)DBNull.Value},
                    { "QlmtMaxClr", request.QlmtMaxClr ?? (object)DBNull.Value},
                    { "AdltTesNam", request.AdltMbrAllow ?? (object)DBNull.Value},
                    { "AdltIsEnaAdl", request.AdltBlock ?? (object)DBNull.Value},
                    { "FuncAllFarCodEdi", request.FuncCollWhileUnapprove ?? (object)DBNull.Value},
                    { "FuncValRatRanOnImp", request.FuncDirectDispatch ?? (object)DBNull.Value},
                    { "UnitKgToLitCon", request.UnitKgToLtrFactor ?? (object)DBNull.Value},
                    { "EntrMulSamTyp", request.EntrSameMilkType ?? (object)DBNull.Value},
                    { "EntrMulDifTyp", request.EntrDiffMilkType ?? (object)DBNull.Value},
                    { "InptInpFat", request.InptFatEditable ?? (object)DBNull.Value},
                    { "InptInpSnf", request.InptSnfEditable ?? (object)DBNull.Value},
                    { "VariVarFat", request.VariVarFat ?? (object)DBNull.Value},
                    { "VariVarSnf", request.VariVarSnf ?? (object)DBNull.Value},
                    { "VariBloQty", request.VariBloQty ?? (object)DBNull.Value},
                    { "MachMacNo", request.MachMacNo ?? (object)DBNull.Value},
                    { "MachMacShi", request.MachMacShi ?? (object)DBNull.Value},
                    { "DsplDisBasOn", request.DsplMapRule ?? (object)DBNull.Value},
                    { "OthrRatChaApp", request.OthrNotes ?? (object)DBNull.Value},
                    { "IsStatus", request.IsStatus },
                    { "ModifiedBy", request.ModifiedBy }
                };

                await repository.UpdateAsync(ConfigSettingQueries.AddConfigSetting, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"ConfigSetting {request.ConfigId} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateConfigSetting for ConfigId: {request.ConfigId}", ex);
                throw;
            }
        }

        public async Task DeleteConfigSetting(int configId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "ConfigId", configId },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(
                    ConfigSettingQueries.AddConfigSetting,
                    requestParams,
                    CommandType.StoredProcedure
                );

                logging.LogInfo($"ConfigSetting with ID {configId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteConfigSetting for ConfigId: {configId}", ex);
                throw;
            }
        }

        public async Task<ConfigSettingResponse?> GetById(int configId)
        {
            try
            {
                logging.LogInfo($"GetById called for ConfigId: {configId}");

                var repo = repositoryFactory.ConnectDapper<ConfigSettingResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<ConfigSettingResponse>(
                    ConfigSettingQueries.GetConfigSettingList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "ConfigId", configId }
                    },
                    null
                );

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for ConfigId: {configId}", ex);
                throw;
            }
        }
  
        public async Task<IListsResponse<ConfigSettingResponse>> GetAll(IListsRequest request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<ConfigSettingResponse>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.All },
                    { "Search", request.Search },
                    { "Limit", request.Limit },
                    { "Offset", request.Offset }
                };

                if (request.Filters != null)
                {
                    foreach (var filter in request.Filters)
                    {
                        parameters.Add(filter.Key, filter.Value ?? DBNull.Value);
                    }
                }

                var result = await repo.QueryAsync<ConfigSettingResponse>(
                    ConfigSettingQueries.GetConfigSettingList,
                    parameters,
                    null
                );

                return new ListsResponse<ConfigSettingResponse>
                {
                    Results = result,
                    Count = result.Count()
                };
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetAll ConfigSettings (paginated)", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ConfigSettingResponse>> GetConfigSettings(ConfigSettingRequest request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<ConfigSettingResponse>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.SpecificFields },
                    { "BusinessId", request.BusinessId },
                    { "CompanyId", request.CompanyId }
                };

                var result = await repo.QueryAsync<ConfigSettingResponse>(
                    ConfigSettingQueries.GetConfigSettingList,
                    parameters,
                    null
                );

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetConfigSettings", ex);
                throw;
            }
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(ConfigSettingRequest request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<CommonLists>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.SpecificFields },
                    { "BusinessId", request.BusinessId }
                };

                var result = await repo.QueryAsync<CommonLists>(
                    ConfigSettingQueries.GetConfigSettingList,
                    parameters,
                    null
                );

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetConfigSettingList", ex);
                throw;
            }
        }

    }
}
