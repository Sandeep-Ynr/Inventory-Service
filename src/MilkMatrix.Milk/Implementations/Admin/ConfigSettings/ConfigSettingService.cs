using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.ConfigSettings;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.ConfigSettings;
using MilkMatrix.Milk.Models.Response.ConfigSettings;
using MilkMatrix.Milk.Models.Response.MPP;

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
                    { "BusinessId", request.BusinessId?? (object)DBNull.Value},
                     { "UnitIds", request.UnitId == "null" ? null : request.UnitId },
                    { "UnitType", request.UnitType ?? (object)DBNull.Value  },
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
                    { "BusinessId", request.BusinessId ?? (object)DBNull.Value },
                    { "UnitIds", request.UnitId ?? (object)DBNull.Value  },
                    { "UnitType", request.UnitType ?? (object)DBNull.Value  },
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
                    //{ "IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value}
                };

                var message = await repository.UpdateAsync(ConfigSettingQueries.AddConfigSetting, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"ConfigSetting {request.BusinessId} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateConfigSetting for ConfigId: {request.BusinessId}", ex);
                throw;
            }
        }

        public async Task DeleteConfigSetting(int BusinessId, string UnitType, string UnitIds)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "BusinessId", BusinessId },
                    { "UnitType", UnitType },
                    { "UnitIds", UnitIds }
                };
                await repository.DeleteAsync(
                    ConfigSettingQueries.AddConfigSetting,
                    requestParams,
                    CommandType.StoredProcedure
                );

                logging.LogInfo($"ConfigSetting with ID {BusinessId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteConfigSetting for ConfigId: {BusinessId}", ex);
                throw;
            }
        }

        public async Task<ConfigSettingResponse?> GetById(int CompanyId)
        {
            try
            {
                logging.LogInfo($"GetById called for CompanyId: {CompanyId}");

                var repo = repositoryFactory.ConnectDapper<ConfigSettingResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<ConfigSettingResponse>(
                    ConfigSettingQueries.GetConfigSettingList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "CompanyId", CompanyId }
                    },
                    null
                );

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for CompanyId: {CompanyId}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<ConfigSettingResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ConfigSettingResponse, int, FiltersMeta>(ConfigSettingQueries.GetConfigSettingList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<ConfigSettingResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
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
